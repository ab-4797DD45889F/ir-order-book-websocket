using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Data;
using IrOrderBook.Services;

const CurrencyCode primary = CurrencyCode.Xbt;
const CurrencyCode secondary = CurrencyCode.Aud;

var apiClientService = new ApiClientService();

var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

// todo: turn it into an infinite loop and exit on pressing Enter key in console (Console.ReadLine())

for (var i = 0; i < 10; i++)
{
    var orderBook = await apiClientService.GetOrderBook(primary, secondary);
    var orderBookWrapper = new OrderBookWrapper(timestamp, orderBook);
    Console.WriteLine(orderBookWrapper);

    // todo: calculate difference between the order books

    timestamp++;
    Thread.Sleep(TimeSpan.FromSeconds(1));
}

// todo: add metrics to see how long it takes to update the order book
