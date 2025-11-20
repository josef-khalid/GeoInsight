using System;
using System.Collections.Generic;
using GeoInsight.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoInsight.Services
{
    public class CountryService : ICountryService
    {
        private readonly IHttpClientFactory _factory;
        public CountryService(IHttpClientFactory factory) => _factory = factory;

        // REST Countries: /v3.1/name/{name}
        public async Task<CountryInfo?> GetCountryByNameAsync(string countryName)
        {
            try
            {
                var client = _factory.CreateClient("RestCountries");
                var url = $"v3.1/name/{Uri.EscapeDataString(countryName)}?fullText=false";
                var res = await client.GetAsync(url);
                if (!res.IsSuccessStatusCode) return null;

                using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
                var root = doc.RootElement;
                if (root.GetArrayLength() == 0) return null;
                var first = root[0];

                var name = first.GetProperty("name").GetProperty("common").GetString() ?? "";
                var population = first.GetProperty("population").GetInt64();
                var area = first.TryGetProperty("area", out var areaEl) ? areaEl.GetDouble() : 0;
                var capital = first.TryGetProperty("capital", out var cap) && cap.GetArrayLength() > 0 ? cap[0].GetString() ?? "" : "";

                // currencies is an object with currency codes as properties
                string currencyCode = "";
                if (first.TryGetProperty("currencies", out var currencies))
                {
                    foreach (var prop in currencies.EnumerateObject())
                    {
                        currencyCode = prop.Name;
                        break;
                    }
                }

                var languages = new List<string>();
                if (first.TryGetProperty("languages", out var langs))
                {
                    foreach (var prop in langs.EnumerateObject())
                    {
                        var lang = prop.Value.GetString();
                        if (lang != null) languages.Add(lang);
                    }
                }

                return new CountryInfo
                {
                    Name = name,
                    Capital = capital,
                    Population = population,
                    Area = area,
                    CurrencyCode = currencyCode,
                    Languages = languages
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
