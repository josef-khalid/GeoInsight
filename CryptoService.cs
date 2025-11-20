using System;
using GeoInsight.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeoInsight.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly IHttpClientFactory _factory;
        public CryptoService(IHttpClientFactory factory) => _factory = factory;

        // CoinGecko simple/price: /api/v3/simple/price?ids=bitcoin,ethereum&vs_currencies={vs}
        public async Task<CryptoInfo?> GetCryptoPricesAsync(string vsCurrency = "usd")
        {
            try
            {
                var client = _factory.CreateClient("CoinGecko");
                var url = $"api/v3/simple/price?ids=bitcoin,ethereum&vs_currencies={Uri.EscapeDataString(vsCurrency)}";
                var res = await client.GetAsync(url);
                if (!res.IsSuccessStatusCode) return null;

                using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
                var root = doc.RootElement;

                decimal btc = 0, eth = 0;
                if (root.TryGetProperty("bitcoin", out var b) && b.TryGetProperty(vsCurrency, out var bval))
                    btc = bval.GetDecimal();
                if (root.TryGetProperty("ethereum", out var e) && e.TryGetProperty(vsCurrency, out var eval))
                    eth = eval.GetDecimal();

                return new CryptoInfo
                {
                    BitcoinPrice = btc,
                    EthereumPrice = eth,
                    VsCurrency = vsCurrency
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
