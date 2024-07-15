
using KristinaBot.BL.Abstracts.APIClientsAbstracts;
using KristinaBot.BL.Abstracts.ServicesAbstracts;
using KristinaBot.DataObjects.Currency;
using Microsoft.Extensions.Caching.Memory;

namespace KristinaBot.BL.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyAPI currencyApi;
        private readonly IMemoryCache cache;
        public CurrencyService(ICurrencyAPI currencyApi, IMemoryCache cache)
        {
            this.currencyApi = currencyApi;
            this.cache = cache;
        }

        public async Task<string> CalculateInAnotherCurrency(string fromCurrency, string toCurrency, int amount)
        {
            cache.TryGetValue("rates", out CurrencyRateResult rateList);
            if(rateList == null)
            {
                rateList = await currencyApi.GetExchangeRates();
                cache.Set("rates", rateList, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(5)));
            }
            if(rateList.Rates.Any())
            {
                if (fromCurrency == "UAH")
                {
                    var rate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == toCurrency);
                    var result = Math.Round(amount / rate.Amount * rate.Units, 2);
                    return $"Today {amount} {fromCurrency} equals {result} {toCurrency}";
                }   
                else if(toCurrency == "UAH")
                {
                    var rate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == fromCurrency);
                    var result = Math.Round(amount * rate.Amount / rate.Units, 2);
                    return $"Today {amount} {fromCurrency} equals {result} {toCurrency}";
                }
                else
                {
                    var firstRate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == fromCurrency);
                    var resultInUah = amount * firstRate.Amount / firstRate.Units;
                    var secondRate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == toCurrency);
                    var result = Math.Round(resultInUah / secondRate.Amount * secondRate.Units, 2);
                    return $"Today {amount} {fromCurrency} equals {result} {toCurrency}";
                }
            }
            else
            {
                return "Something went wrong, please ask again!";
            }
        }

        public async Task<string> GetCurrentExchangeRate(string firstCurrency, string secondCurrency)
        {
            cache.TryGetValue("rates", out CurrencyRateResult rateList);
            if (rateList == null)
            {
                rateList = await currencyApi.GetExchangeRates();
                cache.Set("rates", rateList, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(5)));
            }

            if(rateList.Rates.Any())
            {
                if(firstCurrency == "UAH")
                {
                    var rate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == secondCurrency);
                    var result = Math.Round(rate.Units / rate.Amount, 2);
                    return $"{firstCurrency} to {secondCurrency} exchange rate today is {result} {secondCurrency} per 1 {firstCurrency} ";
                }
                else if(secondCurrency == "UAH")
                {
                    var rate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == firstCurrency);
                    var result = Math.Round(rate.Amount, 2);
                    return $"{firstCurrency} to {secondCurrency} exchange rate today is {result} {secondCurrency} per {rate.Units} {firstCurrency} ";
                }
                else
                {
                    var firstRate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == firstCurrency);
                    var secondRate = rateList.Rates.FirstOrDefault(x => x.CurrencyCodeL == secondCurrency);
                    var result = Math.Round(firstRate.Amount / firstRate.Units * secondRate.Units / secondRate.Amount, 2);
                    return $"{firstCurrency} to {secondCurrency} exchange rate today is {result} {secondCurrency} per {firstRate.Units} {firstCurrency} ";
                }
            }
            else
            {
                return "Something went wrong, please ask again!";
            }
        }
    }
}