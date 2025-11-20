using GeoInsight.Models;
using System.Threading.Tasks;

namespace GeoInsight.Services
{
    public interface IWeatherService
    {
        Task<WeatherInfo?> GetCurrentWeatherAsync(double lat, double lon);
    }
}
