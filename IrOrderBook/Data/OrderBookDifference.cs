using IndependentReserve.DotNetClientApi.Data;

namespace IrOrderBook.Data;

public class OrderBookDifferenceItem
{
    public decimal Price { get; set; }
    public decimal Volume { get; set; }
}

public class OrderBookDifference
{
    public long Index { get; set; }

    // todo: serialized json should not contain this field
    public CurrencyCode Primary { get; set; }

    // todo: serialized json should not contain this field
    public CurrencyCode Secondary { get; set; }

    public OrderBookDifferenceItem[] BuyOrders { get; set; }

    public OrderBookDifferenceItem[] SellOrders { get; set; }

    public override string ToString() => $"Diff{Primary}{Secondary}: {Index}: {BuyOrders.Length} buy orders and {SellOrders.Length} sell orders";
}
