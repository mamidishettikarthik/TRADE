using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Asset.cs

// Asset.cs
public  class Asset
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public Asset(string name, string symbol, decimal price, int quantity)
    {
        Name = name;
        Symbol = symbol;
        Price = price;
        Quantity = quantity;
    }
}

public class Equity : Asset
{
    public string Sector { get; set; }
    public decimal MarketCap { get; set; }

    public Equity(string name, string symbol, decimal price, int quantity, string sector, decimal marketCap)
        : base(name, symbol, price, quantity)
    {
        Sector = sector;
        MarketCap = marketCap;
    }
}

public class Bond : Asset
{
    public decimal CouponRate { get; set; }
    public DateTime MaturityDate { get; set; }

    public Bond(string name, string symbol, decimal price, int quantity, decimal couponRate, DateTime maturityDate)
        : base(name, symbol, price, quantity)
    {
        CouponRate = couponRate;
        MaturityDate = maturityDate;
    }
}

public class Commodity : Asset
{
    public string Unit { get; set; }
    public string Exchange { get; set; }

    public Commodity(string name, string symbol, decimal price, int quantity, string unit, string exchange)
        : base(name, symbol, price, quantity)
    {
        Unit = unit;
        Exchange = exchange;
    }
}

