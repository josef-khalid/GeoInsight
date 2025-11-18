using GeoInsight.Models;
using GeoInsight.Services;
using System;
using System.Threading.Tasks;

namespace GeoInsight.Business
{
    public class GeoInsightService : IGeoInsightService
    {
        private readonly IGeocodingService _geocode;
        private readonly IWeatherService _weather;
        private readonly ICountryService _country;
        private readonly ICryptoService _crypto;

        public GeoInsightService(IGeocodingService geocode, IWeatherService weather, ICountryService country, ICryptoService crypto)
        {
            _geocode = geocode;
            _weather = weather;
            _country = country;
            _crypto = crypto;
        }

        public async Task<GeoInsightViewModel> BuildAsync(string query)
        {
            var vm = new GeoInsightViewModel { Query = query };

            try
            {
                var coord = await _geocode.GetCoordinatesAsync(query);
                if (coord == null)
                {
                    vm.Error = "Kunde inte hitta platsen. Prova med stadens eller landets namn.";
                    return vm;
                }

                var (lat, lon, countryName) = coord.Value;

                // Country info (by returned country name)
                var countryInfo = await _country.GetCountryByNameAsync(countryName ?? query);
                vm.Country = countryInfo;

                // Weather info by coordinates
                var weather = await _weather.GetCurrentWeatherAsync(lat, lon);
                vm.Weather = weather;

                // Crypto in country's currency if found, else default to usd
                var vs = countryInfo?.CurrencyCode?.ToLower() ?? "usd";
                var crypto = await _crypto.GetCryptoPricesAsync(vs);
                // If CoinGecko doesn't support that currency, try USD as fallback
                if (crypto == null && vs != "usd")
                {
                    crypto = await _crypto.GetCryptoPricesAsync("usd");
                }
                vm.Crypto = crypto;
            }
            catch (Exception ex)
            {
                vm.Error = $"Ett fel uppstod: {ex.Message}";
            }

            return vm;
        }
    }
}
