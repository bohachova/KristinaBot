using KristinaBot.DataObjects.Weather;
using Location = KristinaBot.DataObjects.Weather.Location;

namespace KristinaBot.BL.Abstracts.APIClientsAbstracts
{
    public interface IWeatherAPI
    {
        Task<WeatherResponse> GetCurrentWeather (Location loc);
    }
}
