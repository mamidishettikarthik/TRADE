using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1system
{
    // User.cs
    

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public List<Asset> Portfolio { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Balance = 10000; // Initial balance for each user
            Portfolio = new List<Asset>();
        }

        public void DisplayPortfolio()
        {
            Console.WriteLine($"\nPortfolio for {Username}:");
            foreach (var asset in Portfolio)
            {
                Console.WriteLine($"- {asset.Name} ({asset.Symbol}): {asset.Quantity} units");
            }
        }
    }

}
