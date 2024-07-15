using KristinaBot.BL.Abstracts.APIClientsAbstracts;
using KristinaBot.DataObjects.Weather;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;

namespace KristinaBot.BL.Services
{
    public class WeatherAPIClient : IWeatherAPI
    {
        private readonly string weatherApiUrl;
        public WeatherAPIClient(IConfiguration configuration)
        {
            IConfigurationSection apiSection = configuration.GetSection("API");
            weatherApiUrl = apiSection.GetSection("WeatherApi").Value;
        }
        public async Task<WeatherResponse> GetCurrentWeather(Location loc)
        {
            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri($"{weatherApiUrl}/forecast?latitude={loc.Latitude.ToString(CultureInfo.InvariantCulture)}&longitude={loc.Longitude.ToString(CultureInfo.InvariantCulture)}&current_weather=true");
            try
            {
                HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherResponse>(result);
            }
            catch
            {
                return new WeatherResponse { CurrentWeather = new CurrentWeather { Temperature = -100 } };
            }
        }
    }
}
