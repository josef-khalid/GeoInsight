namespace GeoInsight.Models
{
    public class CryptoInfo
    {
        public decimal BitcoinPrice { get; set; }  
        public decimal EthereumPrice { get; set; }
        public string VsCurrency { get; set; } = "usd";
    }
}
