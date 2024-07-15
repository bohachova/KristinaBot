

namespace KristinaBot.BL.Abstracts.ServicesAbstracts
{
    public interface IWeatherService
    {
        public Task<string> GetCurrentWeather(string locationName, double latitude, double longitude);
    }
}
