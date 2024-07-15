

using KristinaBot.DataObjects.Enums;

namespace KristinaBot.BL.Utilities
{
    public static class WeatherCodeConverter
    {
        readonly static HashSet<int> cloudRange = new HashSet<int>(Enumerable.Range(1, 3));
        readonly static HashSet<int> fogRange = new HashSet<int>(Enumerable.Range(45, 4));
        readonly static HashSet<int> rainRange = new HashSet<int>(Enumerable.Range(51, 17));
        readonly static HashSet<int> rainRange2 = new HashSet<int>(Enumerable.Range(80, 3));
        readonly static HashSet<int> snowRange = new HashSet<int>(Enumerable.Range(71, 7));
        readonly static HashSet<int> snowRange2 = new HashSet<int>(Enumerable.Range(85, 2));

        public static WeatherCode ConvertWeatherCode (int code)
        {
            if (cloudRange.Contains(code))
                return WeatherCode.Cloudy;
            else if (fogRange.Contains(code))
                return WeatherCode.Foggy;
            else if (rainRange.Contains(code) || rainRange2.Contains(code))
                return WeatherCode.Raining;
            else if (snowRange.Contains(code) || snowRange2.Contains(code))
                return WeatherCode.Snowing;
            else
                return WeatherCode.Clear;
        }
    }
}
