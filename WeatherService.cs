using System;
using GeoInsight.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GeoInsight.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IHttpClientFactory _factory;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IHttpClientFactory factory, ILogger<WeatherService> logger)
        {
            _factory = factory;
            _logger = logger;
        }

        public async Task<WeatherInfo?> GetCurrentWeatherAsync(double lat, double lon)
        {
            try
            {
                var client = _factory.CreateClient("OpenMeteo");
                var url = BuildWeatherUrl(lat, lon);

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) 
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Weather API error: {StatusCode}, {Content}", response.StatusCode, errorContent);
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content)) return null;

                var weatherInfo = ParseWeather(content);
                if (weatherInfo != null)
                    _logger.LogInformation("Weather DTO: {@WeatherInfo}", weatherInfo);

                return weatherInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch weather data");
                return null;
            }
        }

        private string BuildWeatherUrl(double lat, double lon)
        {
            return $"v1/forecast?latitude={lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                   $"&longitude={lon.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                   $"&current_weather=true&hourly=relativehumidity_2m&timezone=auto";
        }

        private WeatherInfo? ParseWeather(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("current_weather", out var cur)) return null;

            double temp = cur.GetProperty("temperature").GetDouble();
            double wind = cur.GetProperty("windspeed").GetDouble();
            int weatherCode = cur.TryGetProperty("weathercode", out var wc) ? wc.GetInt32() : 0;

            double humidity = 0;
            if (root.TryGetProperty("hourly", out var hourly) &&
                hourly.TryGetProperty("relativehumidity_2m", out var humidityArray) &&
                humidityArray.GetArrayLength() > 0)
            {
                humidity = humidityArray[0].GetDouble();
            }

            return new WeatherInfo
            {
                TemperatureC = Math.Round(temp, 1),
                WindSpeedKph = Math.Round(wind, 1),
                HumidityPercent = Math.Round(humidity, 0),
                WeatherDescription = GetWeatherDescription(weatherCode)
            };
        }

        private string GetWeatherDescription(int code) => code switch
        {
            0 => "Klar himmel",
            1 => "Mestadels klart",
            2 => "Delvis molnigt",
            3 => "Mulet",
            45 => "Dimma",
            48 => "Deponerad rimfrost",
            51 => "Lätt duggregn",
            53 => "Måttlig duggregn",
            55 => "Tät duggregn",
            56 => "Lätt underkylt duggregn",
            57 => "Tät underkylt duggregn",
            61 => "Lätt regn",
            63 => "Måttligt regn",
            65 => "Kraftigt regn",
            66 => "Lätt underkylt regn",
            67 => "Kraftigt underkylt regn",
            71 => "Lätt snöfall",
            73 => "Måttligt snöfall",
            75 => "Kraftigt snöfall",
            77 => "Snökorn",
            80 => "Lätt regnskur",
            81 => "Måttlig regnskur",
            82 => "Kraftig regnskur",
            85 => "Lätt snöby",
            86 => "Kraftig snöby",
            95 => "Åska",
            96 => "Åska med hagel",
            99 => "Åska med kraftigt hagel",
            _ => "Okänt väder"
        };
    }
}
