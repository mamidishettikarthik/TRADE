using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1system
{
    public class Market
    {
        public List<Equity> Equities { get; private set; }
        public List<Bond> Bonds { get; private set; }
        public List<Commodity> Commodities { get; private set; }

        public Market()
        {
            Equities = new List<Equity>();
            Bonds = new List<Bond>();
            Commodities = new List<Commodity>();

            // Initialize market with sample assets
            InitializeMarket();
        }

        private void InitializeMarket()
        {
            // Add sample equities
            Equities.Add(new Equity("Apple Inc.", "AAPL", 150.25m, 1000, "Technology", 200000000000));
            Equities.Add(new Equity("Microsoft Corp.", "MSFT", 275.50m, 800, "Technology", 180000000000));

            // Add sample bonds
            Bonds.Add(new Bond("US Treasury Bond", "US-10Y", 110.75m, 500, 2.5m, DateTime.Now.AddYears(10)));
            Bonds.Add(new Bond("Corporate Bond A", "CorpA", 95.20m, 300, 3.0m, DateTime.Now.AddYears(7)));

            // Add sample commodities
            Commodities.Add(new Commodity("Gold", "Au", 1800.50m, 100, "ounce", "COMEX"));
            Commodities.Add(new Commodity("Crude Oil", "CL", 75.30m, 500, "barrel", "NYMEX"));
        }
    }
}
