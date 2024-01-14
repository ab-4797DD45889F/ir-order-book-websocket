using IndependentReserve.DotNetClientApi.Data;

namespace IrOrderBook.Data;

public class OrderBookWrapper(long nonce, OrderBook original)
{
    public OrderBook Original { get; private set; } = original;

    public override string ToString() => $"{Original.PrimaryCurrencyCode}{Original.SecondaryCurrencyCode}: {nonce}: {original.BuyOrders.Count} buy orders and {original.SellOrders.Count} sell orders";
}
