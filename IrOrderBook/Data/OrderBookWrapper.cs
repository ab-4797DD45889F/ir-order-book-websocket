using IndependentReserve.DotNetClientApi.Data;

namespace IrOrderBook.Data;

public class OrderBookWrapper(long index, OrderBook original)
{
    /// <summary>
    /// Unique timestamp to distinguish order books between each other.
    /// </summary>
    public long Index { get; set; } = index;

    public OrderBook Original { get; private set; } = original;

    public override string ToString() => $"{Original.PrimaryCurrencyCode}{Original.SecondaryCurrencyCode}: {index}: {original.BuyOrders.Count} buy orders and {original.SellOrders.Count} sell orders";
}
