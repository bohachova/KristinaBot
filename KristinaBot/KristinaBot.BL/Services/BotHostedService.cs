using KristinaBot.BL.Abstracts.ServicesAbstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using IHelpService = KristinaBot.BL.Abstracts.ServicesAbstracts.IHelpService;

namespace KristinaBot.BL.Services
{
    public class BotHostedService : IHostedService
    {
        private readonly string token;
        private static ITelegramBotClient botClient;
        private static ReceiverOptions receiverOptions;
        private readonly IServiceProvider serviceProvider;
        private List<string> helpKeyboardOptions;
        public BotHostedService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.serviceProvider = serviceProvider;
            token = configuration.GetSection("BotToken").Value;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            botClient = new TelegramBotClient(token);
            receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message
                },
                ThrowPendingUpdates = true
            };
            botClient.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions, cancellationToken);
            var me = await botClient.GetMeAsync();
            await Task.Delay(-1);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var scope = serviceProvider.CreateScope();

            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                    {
                        var message = update.Message;
                        var chat = message.Chat;
                        switch (message.Type)
                        {
                            case MessageType.Text:
                            {
                                if (message.Text == "/start")
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Hi! You can view the list of possible questions using the “help” command, or just ask your question if you are already informed.");
                                    return;
                                }
                                else if (message.Text.Trim() == "help")
                                {
                                    var service = scope.ServiceProvider.GetRequiredService<IHelpService>();
                                    var helpKeyboard = service.ShowHelpKeyboard();
                                    helpKeyboardOptions = service.GetHelpKeyboardOptions();
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Here are the suggested options for you:",
                                        replyMarkup: helpKeyboard);
                                    return;
                                }
                                else if (helpKeyboardOptions != null && helpKeyboardOptions.Contains(message.Text))
                                {
                                    if (message.Text == helpKeyboardOptions[0])
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Please write: weather in (location name).");
                                        return;
                                    }
                                    else if (message.Text == helpKeyboardOptions[1])
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Please write: exchange rate. And indicate which currencies you are interested in.");
                                        return;
                                    }
                                    else if (message.Text == helpKeyboardOptions[2])
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Please write: convert. And indicate the currencies and amount to convert.");
                                        return;
                                    }
                                    return;
                                }
                                else
                                {
                                    var commandIdentService = scope.ServiceProvider.GetRequiredService<ICommandIdentifierService>();
                                    var command = await commandIdentService.IdentifyRequest(message.Text);

                                    if (command[0] == "weather")
                                    {
                                        var service = scope.ServiceProvider.GetRequiredService<IWeatherService>();
                                        var messageWeather = await service.GetCurrentWeather(command[1], double.Parse(command[2].Replace(".", ",")), double.Parse(command[3].Replace(".", ",")));
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            messageWeather);
                                        return;
                                    }
                                    else if (command[0] == "exchange" && command[3] == "1")
                                    {
                                        var service = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
                                        var messageCurRate = await service.GetCurrentExchangeRate(command[1], command[2]);
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            messageCurRate);
                                        return;
                                    }
                                    else if (command[0] == "exchange" && command[3] != "1")
                                    {
                                        var service = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
                                        var messageCalc = await service.CalculateInAnotherCurrency(command[1], command[2], int.Parse(command[3]));
                                        await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        messageCalc);
                                        return;
                                    }
                                    else if (command[0] == "1")
                                    {
                                        await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Please clarify your question.");
                                        return;
                                    }
                                    else if (command[0] == "0")
                                    {
                                        await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Sorry, I can't help you. To view the proposed questions, please write 'help'. ");
                                        return;
                                    }
                                    return;
                                }
                            }
                            default:
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Only text messages!");
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
