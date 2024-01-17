using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Services;

const CurrencyCode primary = CurrencyCode.Xbt;
const CurrencyCode secondary = CurrencyCode.Aud;

var broadcastService = new ConsoleBroadcastService(); // this will be the real websocket broadcast service

var trackingService = new SingleOrderBookTrackingService(primary, secondary, broadcastService);

var cts = new CancellationTokenSource();

await trackingService.Start(cts.Token);

Console.WriteLine("Press Enter to quit...");
Console.ReadLine();
cts.Cancel();

// todo: add metrics to see how long it takes to update the order book

// todo: potential optimisation is to track only first pages of the order book

// todo: create a web project a that will have an endpoint to return the order book dto and that will broadcast diffrence via websockets
