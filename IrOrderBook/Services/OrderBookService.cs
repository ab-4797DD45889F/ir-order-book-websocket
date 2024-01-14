using IrOrderBook.Data;

namespace IrOrderBook.Services;

public class OrderBookService
{
    public OrderBookDifference GetDifference(long index, OrderBookDto left, OrderBookDto right)
    {
        // make sure that left and right are not null
        if (left == null || right == null)
        {
            throw new ArgumentException("Both left and right must be not null");
        }

        // make sure that primary and secondary currency codes match for left and right order books, because otherwise we can't calculate the difference
        if (left.Primary != right.Primary || left.Secondary != right.Secondary)
        {
            throw new ArgumentException("Primary and Secondary currency codes do not match for the left and right order books");
        }

        var diff = new OrderBookDifference();
        diff.Nonce = index;

        diff.Primary = left.Primary;
        diff.Secondary = right.Primary;
        diff.BuyOrders = GetDifferenceForCollection(left.BuyOrders, right.BuyOrders);
        diff.SellOrders = GetDifferenceForCollection(left.SellOrders, right.SellOrders);
        return diff;
    }

    private static OrderBookDtoItem[] GetDifferenceForCollection(OrderBookDtoItem[] leftOrders, OrderBookDtoItem[] rightOrders)
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
        var differenceSet = difference.Select(p => new OrderBookDtoItem { Price = p.Key, Volume = p.Value }).ToArray();
        return differenceSet;
    }
}
