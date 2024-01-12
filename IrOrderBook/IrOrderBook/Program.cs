using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Data;
using IrOrderBook.Services;
using System.Text.Json;

const CurrencyCode primary = CurrencyCode.Xbt;
const CurrencyCode secondary = CurrencyCode.Aud;

var apiClientService = new ApiClientService();

var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
OrderBookWrapper orderBook = null;
// todo: turn it into an infinite loop and exit on pressing Enter key in console (Console.ReadLine())

for (var i = 0; i < 10; i++)
{
    var newOrderBook = await apiClientService.GetOrderBook(primary, secondary);

    var newOrderBookWrapper = new OrderBookWrapper(timestamp, newOrderBook);
    Console.WriteLine(newOrderBookWrapper);

    if (orderBook != null)
    {
        var difference = OrderBookDifference.Get(timestamp, orderBook.Original, newOrderBook);
        Console.WriteLine(difference);

        // Writing difference object as JSON
        var differenceJson = JsonSerializer.Serialize(difference);
        Console.WriteLine(differenceJson);
    }

    orderBook = newOrderBookWrapper;
    timestamp++;

    Console.WriteLine();
    Thread.Sleep(TimeSpan.FromSeconds(1));
}

// todo: add metrics to see how long it takes to update the order book
