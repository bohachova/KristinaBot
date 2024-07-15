using KristinaBot.BL.Abstracts.APIClientsAbstracts;
using KristinaBot.BL.Utilities;
using KristinaBot.DataObjects.Weather;
using KristinaBot.BL.Abstracts.ServicesAbstracts;

namespace KristinaBot.BL.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherAPI weatherAPI;
        public WeatherService(IWeatherAPI weatherAPI)
        {
            this.weatherAPI = weatherAPI;
        }
        public async Task<string> GetCurrentWeather (string locationName, double latitude, double longitude)
        {
            Location loc = new Location { Latitude = latitude, Longitude = longitude, Name = locationName };
            var weatherResponse = await weatherAPI.GetCurrentWeather(loc);
            var weatherCode = WeatherCodeConverter.ConvertWeatherCode(weatherResponse.CurrentWeather.WeatherCode);
            if (weatherResponse.CurrentWeather.Temperature != -100)
                return $"Current weather in {loc.Name}: {weatherResponse.CurrentWeather.Temperature}, {weatherCode}";
            else
                return "Something went wrong, please ask again!";
        }
    }
}
