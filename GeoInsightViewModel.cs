namespace GeoInsight.Models
{
    public class GeoInsightViewModel
    {
        public string Query { get; set; } = string.Empty;
        public WeatherInfo? Weather { get; set; }
        public CountryInfo? Country { get; set; }
        public CryptoInfo? Crypto { get; set; }
        public string? Error { get; set; }
    }
}
