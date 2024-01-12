using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Data;
using System.Text.Json;
using IrOrderBook.Interfaces;

namespace IrOrderBook.Services;

/// <summary>
/// This service tracks single pair (for instance Xbt/Aud) order book, and broadcasts updates.
/// </summary>
public class SingleOrderBookTrackingService
{
    private readonly CurrencyCode _primary;
    private readonly CurrencyCode _secondary;
    private readonly IBroadcastService _broadcastService;
    private readonly OrderBookService _orderBookService = new OrderBookService();
    private readonly ApiClientService _apiClientService = new ApiClientService();

    private long _timestamp;
    private OrderBookWrapper _currentOrderBook;

    private string ChannelName => $"{_primary}{_secondary}Diff";

    /// <summary>
    /// This service tracks single pair (for instance Xbt/Aud) order book, and broadcasts updates.
    /// </summary>
    public SingleOrderBookTrackingService(CurrencyCode primary, CurrencyCode secondary, IBroadcastService broadcastService)
    {
        _primary = primary;
        _secondary = secondary;
        _broadcastService = broadcastService;
    }

    public async Task Start(CancellationToken cancellationToken)
    {
        _timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        while (!cancellationToken.IsCancellationRequested)
        {
            var newOrderBook = await _apiClientService.GetOrderBook(_primary, _secondary);

            var newOrderBookWrapper = new OrderBookWrapper(_timestamp, newOrderBook);
            // Console.WriteLine(newOrderBookWrapper);

            if (_currentOrderBook != null)
            {
                var difference = _orderBookService.GetDifference(_timestamp, _currentOrderBook.Original, newOrderBook);
                // Console.WriteLine(difference);

                // Writing difference object as JSON
                var differenceJson = JsonSerializer.Serialize(difference);

                _broadcastService.Broadcast(ChannelName, differenceJson);
            }

            _currentOrderBook = newOrderBookWrapper;
            _timestamp++;

            // Console.WriteLine();
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}
