using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Data;
using IrOrderBook.Extensions;
using IrOrderBook.Interfaces;

namespace IrOrderBook.Services;

/// <summary>
/// This service tracks single pair order book (for instance Xbt/Aud), and broadcasts updates.
/// </summary>
public class SingleOrderBookTrackingService
{
    private readonly CurrencyCode _primary;
    private readonly CurrencyCode _secondary;
    private readonly IBroadcastService _broadcastService;
    private readonly OrderBookService _orderBookService = new OrderBookService();
    private readonly ApiClientService _apiClientService = new ApiClientService();

    private long _timestamp;

    /// <summary>
    /// Requesting the order book from the API takes some time, so we request order book more often that send differences.
    /// </summary>
    private readonly TimeSpan UpdateApiOrderBookInterval = TimeSpan.FromMicroseconds(555);

    /// <summary>
    /// We will broadcast the differences every second.
    /// </summary>
    private readonly TimeSpan BroadcastOrderDifferencesInterval = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The last known raw order book that we get from the API.
    /// </summary>
    private OrderBookWrapper _rawOrderBook;

    /// <summary>
    /// The last known and processed version of the order book, processed means that the difference was calculated and broadcast.
    /// Represented as a lightweight dto class.
    /// </summary>
    private OrderBookDto _currentOrderBookDto;

    private string PairName => $"{_primary}{_secondary}";
    private string OrderBookDifferenceChannelName => $"{PairName}";

    /// <summary>
    /// This service tracks single pair (for instance Xbt/Aud) order book, and broadcasts updates.
    /// </summary>
    public SingleOrderBookTrackingService(CurrencyCode primary, CurrencyCode secondary, IBroadcastService broadcastService)
    {
        _primary = primary;
        _secondary = secondary;
        _broadcastService = broadcastService;
    }

    // todo: should return order book as of order book items, without order book type etc.
    // todo: this should update order book in the background
    // todo: and there should be another infinite loop that would get current order book and calculate difference, and it should not make requests to API

    /// <summary>
    /// Returns the current (last known) order book.
    /// </summary>
    public OrderBookDto GetOrderBook() => _currentOrderBookDto;

    /// <summary>
    /// Requests the first version of the order book from the API.
    /// And starts two infinite loops:
    /// - to monitor the changes
    /// - and to generate the differences.
    /// </summary>
    public async Task Start(CancellationToken cancellationToken)
    {
        // getting the first version of the order book from the API
        var firstOrderBook = await _apiClientService.GetOrderBook(_primary, _secondary);
        _rawOrderBook = new OrderBookWrapper(_timestamp, firstOrderBook);

        _timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        _currentOrderBookDto = _rawOrderBook.ToDto(_timestamp);

        // starting an infinite loop to update raw order book taken from the API
        Task.Run(() => UpdateRawOrderBookInfiniteLoop(cancellationToken));

        // starting an infinite loop to track and broadcast order book changes
        Task.Run(() => TrackChangesInfiniteLoop(cancellationToken));
    }

    private async Task UpdateRawOrderBookInfiniteLoop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(UpdateApiOrderBookInterval, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            try
            {
                // wrapping this with try catch, because we don't want the entire service to quit because one request to API fails
                var newOrderBook = await _apiClientService.GetOrderBook(_primary, _secondary);
                _rawOrderBook = new OrderBookWrapper(_timestamp, newOrderBook);
                // Console.WriteLine($"update {_rawOrderBook}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"update failed for pair {PairName} {ex.Message}");
            }
        }

        Console.WriteLine($"Background job completed: {nameof(UpdateRawOrderBookInfiniteLoop)}");
    }

    private async Task TrackChangesInfiniteLoop(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(BroadcastOrderDifferencesInterval, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            _timestamp++;
            var newOrderBookDto = _rawOrderBook.ToDto(_timestamp);
            var difference = _orderBookService.GetDifference(_timestamp, _currentOrderBookDto, newOrderBookDto);

            _currentOrderBookDto = newOrderBookDto;

            // todo: this should probably be broadcast in async form, but we will return to this later
            // broadcast difference object as JSON
            var differenceJson = difference.ToJson();
            _broadcastService.Broadcast(OrderBookDifferenceChannelName, differenceJson);
        }

        Console.WriteLine($"Background job completed: {nameof(TrackChangesInfiniteLoop)}");
    }
}
