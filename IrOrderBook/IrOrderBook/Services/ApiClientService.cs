using IndependentReserve.DotNetClientApi;
using IndependentReserve.DotNetClientApi.Data;

namespace IrOrderBook.Services;

public class ApiClientService
{
    private const string BaseUrl = "https://api.independentreserve.com";

    public async Task<OrderBook> GetOrderBook(CurrencyCode primary, CurrencyCode secondary)
    {
        using var client = Client.CreatePublic(BaseUrl); // we will create client every time we make request
        var orderBook = await client.GetOrderBookAsync(primary, secondary);
        return orderBook;
    }
}
