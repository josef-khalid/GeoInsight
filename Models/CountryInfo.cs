using System.Collections.Generic;

namespace GeoInsight.Models
{
    public class CountryInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Capital { get; set; } = string.Empty;
        public long Population { get; set; }
        public double Area { get; set; } // km^2
        public string CurrencyCode { get; set; } = string.Empty;
        public List<string> Languages { get; set; } = new();
    }
}

