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

        
    }
}
