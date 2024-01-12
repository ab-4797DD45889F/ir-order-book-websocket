using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Data;

namespace IrOrderBook.Services;

public class OrderBookService
{
    public OrderBookDifference GetDifference(long index, OrderBook left, OrderBook right)
    {
        // make sure that left and right are not null
        if (left == null || right == null)
        {
            throw new ArgumentException("Both left and right must be not null");
        }

        // make sure that primary and secondary currency codes match for left and right order books, because otherwise we can't calculate the difference
        if (left.PrimaryCurrencyCode != right.PrimaryCurrencyCode || left.SecondaryCurrencyCode != right.SecondaryCurrencyCode)
        {
            throw new ArgumentException("Primary and Secondary currency codes do not match for the left and right order books");
        }

        var diff = new OrderBookDifference();
        diff.Index = index;

        diff.Primary = left.PrimaryCurrencyCode;
        diff.Secondary = right.SecondaryCurrencyCode;
        diff.BuyOrders = GetDifferenceForCollection(left.BuyOrders, right.BuyOrders);
        diff.SellOrders = GetDifferenceForCollection(left.SellOrders, right.SellOrders);
        return diff;
    }

    private static OrderBookDifferenceItem[] GetDifferenceForCollection(List<OrderBookItem> leftOrders, List<OrderBookItem> rightOrders)
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
