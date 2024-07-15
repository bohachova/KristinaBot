using KristinaBot.BL.Abstracts.APIClientsAbstracts;
using KristinaBot.DataObjects.Currency;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace KristinaBot.BL.Services
{
    public class CurrencyAPIClient : ICurrencyAPI
    {
        private readonly string currencyApiUrl;
        public CurrencyAPIClient(IConfiguration configuration)
        {
            IConfigurationSection apiSection = configuration.GetSection("API");
            currencyApiUrl = apiSection.GetSection("CurrencyApi").Value;
        }

        public async Task<CurrencyRateResult> GetExchangeRates()
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(currencyApiUrl);
            try
            {
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                var result = await response.Content.ReadAsStringAsync();
                var rateList = JsonConvert.DeserializeObject <List<CurrencyRate>>(result);
                return new CurrencyRateResult { Rates = rateList };
            }
            catch (Exception ex) 
            {
                return new CurrencyRateResult();
            }
        }
    }
}
