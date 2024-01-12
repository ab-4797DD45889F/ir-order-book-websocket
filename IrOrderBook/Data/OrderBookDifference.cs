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
    public CurrencyCode Primary { get; set; }
    public CurrencyCode Secondary { get; set; }
    public OrderBookDifferenceItem[] BuyOrders { get; set; }
    public OrderBookDifferenceItem[] SellOrders { get; set; }

    public override string ToString() => $"Diff{Primary}{Secondary}: {Index}: {BuyOrders.Length} buy orders and {SellOrders.Length} sell orders";

    // todo: move this to OrderBookService

    public static OrderBookDifference Get(long index, OrderBook left, OrderBook right)
    {
        // todo: make sure that left and right are not null
        // todo: make sure that primary and secondary currency codes match for left and right order books, because otherwise we can't calculate the difference

        var diff = new OrderBookDifference();
        diff.Index = index;

        diff.Primary = left.PrimaryCurrencyCode;
        diff.Secondary = right.SecondaryCurrencyCode;
        diff.BuyOrders = Get(left.BuyOrders, right.BuyOrders);
        diff.SellOrders = Get(left.SellOrders, right.SellOrders);
        return diff;
    }

    private static OrderBookDifferenceItem[] Get(List<OrderBookItem> leftOrders, List<OrderBookItem> rightOrders)
    {
        // Convert the 'left' and 'right' orders to dictionaries for easier lookup
        var leftDict = leftOrders.ToDictionary(x => x.Price, x => x.Volume);
        var rightDict = rightOrders.ToDictionary(x => x.Price, x => x.Volume);

        // Create a dictionary to hold the difference
        var difference = new Dictionary<decimal, decimal>();

        // Calculate the differences
        foreach (var price in leftDict.Keys.Union(rightDict.Keys).OrderBy(key => key))
        {
            var leftVolume = leftDict.GetValueOrDefault(price, 0);
            var rightVolume = rightDict.GetValueOrDefault(price, 0);

            // excluding those volumes that haven't changed
            var diff = rightVolume - leftVolume;

            if (diff != 0m)
            {
                difference.Add(price, diff);
            }
        }

        // Convert the difference dictionary back to a set of Orders
        var differenceSet = difference.Select(p => new OrderBookDifferenceItem { Price = p.Key, Volume = p.Value }).ToArray();
        return differenceSet;
    }
}
