using System.Threading.Tasks;

namespace GeoInsight.Services
{
    public interface IGeocodingService
    {
        Task<(double lat, double lon, string? country)?> GetCoordinatesAsync(string place);
    }
}

