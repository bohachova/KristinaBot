

namespace KristinaBot.BL.Abstracts.ServicesAbstracts
{
    public interface ICurrencyService
    {
        Task<string> GetCurrentExchangeRate (string firstCurrency, string secondCurrency);
        Task<string> CalculateInAnotherCurrency(string fromCurrency, string toCurrency, int amount);
    }
}
