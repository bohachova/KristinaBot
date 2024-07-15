using Newtonsoft.Json;

namespace KristinaBot.DataObjects.Weather
{
    public class CurrentWeather
    {
        [JsonProperty("temperature")]
        public double Temperature { get; set; }
        [JsonProperty("weathercode")]
        public int WeatherCode { get; set; }
       
    }
}
