using System.Collections.Generic;

namespace CurrencyTracker.Models
{
    public class CurrencyResponse
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
