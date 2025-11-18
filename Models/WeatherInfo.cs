namespace GeoInsight.Models
{
    public class WeatherInfo
    {
        public double TemperatureC { get; set; }
        public double WindSpeedKph { get; set; }
        public double HumidityPercent { get; set; }
        public string WeatherDescription { get; set; } = string.Empty;
    }
}
