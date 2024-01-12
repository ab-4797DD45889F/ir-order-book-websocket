using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Services;

const CurrencyCode primary = CurrencyCode.Usdt;
const CurrencyCode secondary = CurrencyCode.Aud;

var broadcastService = new ConsoleBroadcastService(); // this will be the real websocket broadcast service

var trackingService = new SingleOrderBookTrackingService(primary, secondary, broadcastService);

Console.WriteLine("Press Enter to quit...");

var cts = new CancellationTokenSource();
Task.Run(() => trackingService.Start(cts.Token));
Console.ReadLine();
cts.Cancel();

// todo: add metrics to see how long it takes to update the order book

// todo: potential optimisation is to track only first pages of the order book
