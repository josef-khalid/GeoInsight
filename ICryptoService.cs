using GeoInsight.Models;
using System.Threading.Tasks;

namespace GeoInsight.Services
{
    public interface ICryptoService
    {
        Task<CryptoInfo?> GetCryptoPricesAsync(string vsCurrency = "usd");
    }
}
