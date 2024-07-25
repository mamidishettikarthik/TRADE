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
using System;
using System.Data;
using System.Data.OracleClient;

public class OracleDataAccess : IDataAccess
{
    private string _connectionString;

    public OracleDataAccess(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void SaveTradeDetails(string tradeDetails)
    {
        using (OracleConnection connection = new OracleConnection(_connectionString))
        {
            string sql = "INSERT INTO TradeDetails (Details) VALUES (:details)";

            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.Parameters.Add(":details", OracleType.VarChar).Value = tradeDetails;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Trade details saved to Oracle database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving trade details: " + ex.Message);
                }
            }
        }
    }

    public string GetTradeDetails(int tradeId)
    {
        string details = null;

        using (OracleConnection connection = new OracleConnection(_connectionString))
        {
            string sql = "SELECT Details FROM TradeDetails WHERE TradeId = :tradeId";

            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.Parameters.Add(":tradeId", OracleType.Int32).Value = tradeId;

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        details = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving trade details: " + ex.Message);
                }
            }
        }

        return details;
    }

    public void UpdateTradeDetails(int tradeId, string newDetails)
    {
        using (OracleConnection connection = new OracleConnection(_connectionString))
        {
            string sql = "UPDATE TradeDetails SET Details = :newDetails WHERE TradeId = :tradeId";

            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.Parameters.Add(":newDetails", OracleType.VarChar).Value = newDetails;
                command.Parameters.Add(":tradeId", OracleType.Int32).Value = tradeId;

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} trade details updated.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating trade details: " + ex.Message);
                }
            }
        }
    }

    public void DeleteTradeDetails(int tradeId)
    {
        using (OracleConnection connection = new OracleConnection(_connectionString))
        {
            string sql = "DELETE FROM TradeDetails WHERE TradeId = :tradeId";

            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.Parameters.Add(":tradeId", OracleType.Int32).Value = tradeId;

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} trade details deleted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deleting trade details: " + ex.Message);
                }
            }
        }
    }
}
