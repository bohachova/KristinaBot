using KristinaBot.BL.Abstracts.ServicesAbstracts;
using Telegram.Bot.Types.ReplyMarkups;

namespace KristinaBot.BL.Services
{
    public class HelpService : IHelpService
    {
        private const string weather = "Show weather forecast";
        private const string currency = "Show currency exchange rate";
        private const string calculate = "Convert amount to another currency";
        public ReplyKeyboardMarkup ShowHelpKeyboard()
        {
            return new ReplyKeyboardMarkup(
                                        new List<KeyboardButton[]>()
                                        {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton(weather)
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton(currency)
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton(calculate)
                                        }
                                        })
            {
                ResizeKeyboard = true
            };
        }
        public List<string> GetHelpKeyboardOptions() => new List<string> { weather, currency, calculate };
    }
}
