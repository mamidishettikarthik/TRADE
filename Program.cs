// Program.cs
using ConsoleApp1system;
using System;
using System.Collections.Generic;

class Program
{
    static Market market = new Market();
    static List<User> users = new List<User>();

    static void Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nWelcome to the Trading System");
            Console.WriteLine("1. Register");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Register();
                    break;
                case "2":
                    Login();
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    break;
            }
        }
    }

    static void Register()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (users.Exists(u => u.Username == username))
        {
            Console.WriteLine("Username already exists. Please choose another username.");
            return;
        }

        User newUser = new User(username, password);
        users.Add(newUser);
        Console.WriteLine("Registration successful.");
    }

    static void Login()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        User user = users.Find(u => u.Username == username && u.Password == password);

        if (user == null)
        {
            Console.WriteLine("Invalid username or password.");
            return;
        }

        Console.WriteLine($"Login successful. Welcome, {user.Username}!");

        bool logout = false;
        while (!logout)
        {
            Console.WriteLine("\n1. View Market");
            Console.WriteLine("2. Buy Asset");
            Console.WriteLine("3. Sell Asset");
            Console.WriteLine("4. View Portfolio");
            Console.WriteLine("5. Logout");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    DisplayMarket();
                    break;
                case "2":
                    BuyAsset(user);
                    break;
                case "3":
                    SellAsset(user);
                    break;
                case "4":
                    user.DisplayPortfolio();
                    break;
                case "5":
                    logout = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose again.");
                    break;
            }
        }
    }

    static void DisplayMarket()
    {
        Console.WriteLine("\nMarket:");
        Console.WriteLine("Equities:");
        foreach (var equity in market.Equities)
        {
            Console.WriteLine($"{equity.Name} ({equity.Symbol}): ${equity.Price} per share");
        }
        Console.WriteLine("\nBonds:");
        foreach (var bond in market.Bonds)
        {
            Console.WriteLine($"{bond.Name} ({bond.Symbol}): ${bond.Price} per bond");
        }
        Console.WriteLine("\nCommodities:");
        foreach (var commodity in market.Commodities)
        {
            Console.WriteLine($"{commodity.Name} ({commodity.Symbol}): ${commodity.Price} per unit");
        }
    }

    static void BuyAsset(User user)
    {
        Console.WriteLine("\nAvailable assets to buy:");
        DisplayMarket();

        Console.Write("\nEnter asset symbol to buy: ");
        string symbol = Console.ReadLine();

        Console.Write("Enter quantity: ");
        int quantity;
        while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity. Please enter a valid positive integer.");
            Console.Write("Enter quantity: ");
        }

        // Find asset in market
        Asset assetToBuy = FindAssetInMarket(symbol);
        if (assetToBuy == null)
        {
            Console.WriteLine("Asset not found in the market.");
            return;
        }

        decimal totalCost = assetToBuy.Price * quantity;
        if (totalCost > user.Balance)
        {
            Console.WriteLine("Insufficient balance to buy the asset.");
            return;
        }

        // Perform transaction
        user.Balance -= totalCost;
        Asset existingAsset = user.Portfolio.Find(a => a.Symbol == symbol);
        if (existingAsset != null)
        {
            existingAsset.Quantity += quantity;
        }
        else
        {
            Asset newAsset = new Asset(assetToBuy.Name, assetToBuy.Symbol, assetToBuy.Price, quantity);
            user.Portfolio.Add(newAsset);
        }

        Console.WriteLine($"Successfully bought {quantity} units of {assetToBuy.Name} ({assetToBuy.Symbol}).");
    }

    static void SellAsset(User user)
    {
        if (user.Portfolio.Count == 0)
        {
            Console.WriteLine("You do not own any assets to sell.");
            return;
        }

        Console.WriteLine("\nAssets in your portfolio:");
        user.DisplayPortfolio();

        Console.Write("\nEnter asset symbol to sell: ");
        string symbol = Console.ReadLine();

        Console.Write("Enter quantity: ");
        int quantity;
        while (!int.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity. Please enter a valid positive integer.");
            Console.Write("Enter quantity: ");
        }

        // Find asset in user's portfolio
        Asset assetToSell = user.Portfolio.Find(a => a.Symbol == symbol);
        if (assetToSell == null || assetToSell.Quantity < quantity)
        {
            Console.WriteLine("You do not have enough units of the asset to sell.");
            return;
        }

        // Find asset in market
        Asset marketAsset = FindAssetInMarket(symbol);
        if (marketAsset == null)
        {
            Console.WriteLine("Asset not found in the market.");
            return;
        }

        // Perform transaction
        user.Balance += marketAsset.Price * quantity;
        assetToSell.Quantity -= quantity;

        Console.WriteLine($"Successfully sold {quantity} units of {assetToSell.Name} ({assetToSell.Symbol}).");
    }

    static Asset FindAssetInMarket(string symbol)
    {
        foreach (var equity in market.Equities)
        {
            if (equity.Symbol == symbol)
                return equity;
        }

        foreach (var bond in market.Bonds)
        {
            if (bond.Symbol == symbol)
                return bond;
        }

        foreach (var commodity in market.Commodities)
        {
            if (commodity.Symbol == symbol)
                return commodity;
        }

        return null;
    }
}
