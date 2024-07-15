using Telegram.Bot.Types.ReplyMarkups;

namespace KristinaBot.BL.Abstracts.ServicesAbstracts
{
    public interface IHelpService
    {
        ReplyKeyboardMarkup ShowHelpKeyboard();
        List<string> GetHelpKeyboardOptions();
    }
}
