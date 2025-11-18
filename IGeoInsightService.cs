using GeoInsight.Models;
using System.Threading.Tasks;

namespace GeoInsight.Business
{
    public interface IGeoInsightService
    {
        Task<GeoInsightViewModel> BuildAsync(string query);
    }
}
