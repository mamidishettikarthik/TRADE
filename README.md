tradeId,symbol,quantity,price
1,AAPL,10,150.00
2,GOOGL,5,2800.00
3,MSFT,15,300.50
4,AMZN,2,3500.00
5,TSLA,8,750.25
6,FB,20,350.00
7,NFLX,12,600.00
8,DIS,7,180.00
9,TSM,25,120.00
10,INTC,10,55.00
11,CSCO,18,60.00
12,NVDA,5,220.00
13,AMD,30,95.00
14,BABA,4,200.00
15,ORCL,22,85.00
16,SNE,3,110.00
17,PFE,14,40.00
18,MRK,9,70.00
19,JNJ,6,160.00
20,PG,11,140.00
21,VZ,17,60.00
22,T,13,30.00
23,KO,19,55.00
24,PEP,8,150.00
25,BRK.B,1,280.00
26,WMT,15,135.00
27,HD,10,320.00
28,LOW,5,200.00
29,COST,2,380.00
30,CRM,20,250.00
31,NFLX,12,550.00
32,NKE,8,120.00
33,ADBE,10,500.00
34,TXN,15,175.00
35,QCOM,6,120.00
36,AMGN,4,240.00
37,BMY,18,60.00
38,MDT,9,90.00
39,LLY,14,420.00
40,AVGO,3,550.00
41,IBM,11,140.00
42,BA,10,200.00
43,GS,5,400.00
44,JPM,7,150.00
45,C,12,70.00
46,MS,4,100.00
47,USB,8,50.00
48,AMT,9,230.00
49,SPG,2,110.00
50,NTRS,3,120.00
________________________________________________________________
|                             Asset                              |
|----------------------------------------------------------------|
| - Name: string                                                 |
| - Symbol: string                                               |
| - Price: decimal                                               |
| - Quantity: int                                                |
|----------------------------------------------------------------|
| + Asset(name: string, symbol: string, price: decimal, quantity: int) |
|________________________________________________________________|

        ^
        |       _________________________________________________________
   _____|______|_______________________            ___________________|
  |            |                       |          |                   |
  |  Equity    |        Bond           |          |    Commodity      |
  |------------|-----------------------|          |-------------------|
  | - Sector: string           |       |          | - Unit: string    |
  | - MarketCap: decimal       |       |          | - Exchange: string|
  |____________________________|       |          |___________________|

        ^
        |          _______________________
   _____|______   |                       |
  |            |  |         User          |
  |------------|  |-----------------------|
  | - Username: string                |
  | - Password: string                |
  | - Balance: decimal                |
  | - Portfolio: List<Asset>          |
  |___________________________________|

        ^
        |          _______________________
   _____|______   |                       |
  |            |  |        Market         |
  |------------|  |-----------------------|
  | - Equities: List<Equity>            |
  | - Bonds: List<Bond>                 |
  | - Commodities: List<Commodity>      |
  |___________________________________|

        |
        |         _______________________
        |        |                       |
        |_______>|        Program        |
                 |-----------------------|
                 |                       |
                 | - Main()               |
                 | - Register()           |
                 | - Login()              |
                 | - DisplayMarket()      |
                 | - BuyAsset()           |
                 | - SellAsset()          |
                 | - FindAssetInMarket()  |
                 |_______________________|
class Program
{
    static void Main()
    {
        // Create an instance of the trade processing factory
        ITradeProcessingFactory factory = new TradeProcessingFactory();

        // Parallel initiation of two trades
        Task<ITrade> trade1Task = Task.Run(() =>
        {
            IFrontOffice frontOffice = factory.CreateFrontOffice();
            return frontOffice.InitiateTrade("AAPL", 100); // Buying 100 shares of AAPL
        });

        Task<ITrade> trade2Task = Task.Run(() =>
        {
            IFrontOffice frontOffice = factory.CreateFrontOffice();
            return frontOffice.InitiateTrade("GOOGL", 50); // Buying 50 shares of GOOGL
        });

        // Middle and Back office processing (concurrent)
        Task middleAndBackOfficeTasks = Task.WhenAll(
            trade1Task.ContinueWith(previousTask =>
            {
                IMiddleOffice middleOffice = factory.CreateMiddleOffice();
                middleOffice.ValidateTrade(previousTask.Result);

                IBackOffice backOffice = factory.CreateBackOffice();
                backOffice.RecordTrade(previousTask.Result);
                backOffice.ConfirmTrade(previousTask.Result);
            }),
            trade2Task.ContinueWith(previousTask =>
            {
                IMiddleOffice middleOffice = factory.CreateMiddleOffice();
                middleOffice.ValidateTrade(previousTask.Result);

                IBackOffice backOffice = factory.CreateBackOffice();
                backOffice.RecordTrade(previousTask.Result);
                backOffice.ConfirmTrade(previousTask.Result);
            })
        );

        // Execute trades after all processing
        Task executeTradesTask = middleAndBackOfficeTasks.ContinueWith(previousTask =>
        {
            trade1Task.Result.ExecuteTrade();
            trade2Task.Result.ExecuteTrade();
        });

        // Wait for all tasks to complete
        Task.WaitAll(trade1Task, trade2Task, executeTradesTask);

        Console.ReadLine();
    }
}
