

using KristinaBot.DataObjects.Currency;

namespace KristinaBot.BL.Abstracts.APIClientsAbstracts
{
    public interface ICurrencyAPI
    {
        Task<CurrencyRateResult> GetExchangeRates();
    }
}
