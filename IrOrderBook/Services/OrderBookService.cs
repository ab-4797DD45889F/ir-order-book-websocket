using IrOrderBook.Data;

namespace IrOrderBook.Services;

public class OrderBookService
{
    public OrderBookDifference GetDifference(OrderBookDto left, OrderBookDto right)
    {
        // make sure that left and right are not null
        if (left == null || right == null)
        {
            throw new ArgumentException("Both left and right must be not null");
        }

        // make sure that primary and secondary currency codes match for left and right order books, because otherwise we can't calculate the difference
        if (left.Pair != right.Pair)
        {
            throw new ArgumentException("Primary and Secondary currency codes do not match for the left and right order books");
        }

        var diff = new OrderBookDifference();
        diff.Nonce = right.Nonce;
        diff.Pair = left.Pair;
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

    public OrderBookDto ApplyDifference(OrderBookDto left, OrderBookDifference difference)
    {
        // make sure that left and difference are not null
        if (left == null || difference == null)
        {
            throw new ArgumentException("Both left order book and difference must be not null");
        }

        // make sure that primary and secondary currency codes match for left and difference, because otherwise we can't reconstruct the right order book
        if (left.Pair != difference.Pair)
        {
            throw new ArgumentException("Primary and Secondary currency codes do not match for the left order book and difference");
        }

        var right = new OrderBookDto();
        right.Nonce = difference.Nonce;
        right.Pair = left.Pair;
        right.BuyOrders = ReconstructCollectionWithDifference(left.BuyOrders, difference.BuyOrders);
        right.SellOrders = ReconstructCollectionWithDifference(left.SellOrders, difference.SellOrders);
        return right;
    }

    private OrderBookDtoItem[] ReconstructCollectionWithDifference(OrderBookDtoItem[] leftOrders, OrderBookDtoItem[] diffOrders)
    {
        var leftDict = leftOrders.ToDictionary(x => x.Price, x => x.Volume);
        var diffDict = diffOrders.ToDictionary(x => x.Price, x => x.Volume);
        /*
         * diff = rightVolume - leftVolume
         * to calculate rightVolume we can rearrange the formula and get
         * rightVolume = diff + leftVolume
         */
        var right = new Dictionary<decimal, decimal>();
        // we need to consider all prices that are in 'leftOrders' or 'diffOrders'
        foreach (var price in leftDict.Keys.Union(diffDict.Keys))
        {
            var leftVolume = leftDict.GetValueOrDefault(price, 0);
            var diff = diffDict.GetValueOrDefault(price, 0);

            var rightVolume = diff + leftVolume;

            // we assume there can't be a negative volume in 'rightOrders'
            if (rightVolume > 0)
            {
                right.Add(price, rightVolume);
            }
        }

        // Convert the right dictionary back to a set of Orders
        var rightSet = right.Select(p => new OrderBookDtoItem { Price = p.Key, Volume = p.Value }).ToArray();
        return rightSet;
    }
}
