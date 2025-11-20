using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoInsight.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly IHttpClientFactory _factory;
        public GeocodingService(IHttpClientFactory factory) => _factory = factory;

        // använder Open-Meteo geocoding: /v1/search?name={place}
        public async Task<(double lat, double lon, string? country)?> GetCoordinatesAsync(string place)
        {
            try
            {
                var client = _factory.CreateClient("OpenMeteoGeo");
                var url = $"v1/search?name={Uri.EscapeDataString(place)}&count=1&language=en&format=json";
                var res = await client.GetAsync(url);
                if (!res.IsSuccessStatusCode) return null;

                using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
                var root = doc.RootElement;
                if (!root.TryGetProperty("results", out var results)) return null;
                if (results.GetArrayLength() == 0) return null;

                var first = results[0];
                var lat = first.GetProperty("latitude").GetDouble();
                var lon = first.GetProperty("longitude").GetDouble();
                var country = first.TryGetProperty("country", out var countryEl) ? countryEl.GetString() : null;
                return (lat, lon, country);
            }
            catch
            {
                return null;
            }
        }
    }
}
