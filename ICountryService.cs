using GeoInsight.Models;
using System.Threading.Tasks;

namespace GeoInsight.Services
{
    public interface ICountryService
    {
        Task<CountryInfo?> GetCountryByNameAsync(string countryName);
    }
}
