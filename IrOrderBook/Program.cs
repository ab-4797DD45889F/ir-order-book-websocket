using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Services;

const CurrencyCode primary = CurrencyCode.Xbt;
const CurrencyCode secondary = CurrencyCode.Aud;

var trackingService = new SingleOrderBookTrackingService(primary, secondary, new ConsoleBroadcastService());

var cts = new CancellationTokenSource();

await trackingService.Start(cts.Token);

Console.WriteLine("Press Enter to quit...");
Console.ReadLine();
cts.Cancel();

// todo: add metrics to see how long it takes to update the order book
// todo: potential optimisation is to track only first page of the order book
