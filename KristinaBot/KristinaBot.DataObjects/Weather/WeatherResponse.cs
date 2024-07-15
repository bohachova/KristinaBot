using Newtonsoft.Json;

namespace KristinaBot.DataObjects.Weather
{
    public class WeatherResponse
    {
        [JsonProperty("current_weather")]
        public CurrentWeather CurrentWeather { get; set; } = new CurrentWeather();
    }
}
