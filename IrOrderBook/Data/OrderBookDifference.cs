using System.Text.Json.Serialization;
using IndependentReserve.DotNetClientApi.Data;

namespace IrOrderBook.Data;

/// <summary>
/// Represents the difference between two order books.
/// Order Book [Nonce -1] + Difference [Nonce] = Order Book [Nonce]
/// </summary>
public class OrderBookDifference
{
    /// <summary>
    /// The latest order book version.
    /// Order Book [Nonce -1] + Difference [Nonce] = Order Book [Nonce]
    /// </summary>
    [JsonPropertyName("n")]
    public long Nonce { get; set; }

    /// <summary>
    /// For information purpose only, it is used in ToString method for example.
    /// Serialized json doesn't contain this field.
    /// </summary>
    [JsonIgnore]
    public string Pair { get; set; }

    /// <summary>
    /// This collection describes the difference in buy orders between two order books.
    /// For each price that appear in both order books, volume field shows the difference in available volume.
    /// Positive volume means that more volume is available for this price.
    /// Negative volume means that less volume is available for this price.
    /// </summary>
    [JsonPropertyName("b")]
    public OrderBookDtoItem[] BuyOrders { get; set; }

    /// <summary>
    /// This collection describes the difference in buy orders between two order books.
    /// For each price that appear in both order books, volume field shows the difference in available volume.
    /// Positive volume means that more volume is available for this price.
    /// Negative volume means that less volume is available for this price.
    /// </summary>
    [JsonPropertyName("s")]
    public OrderBookDtoItem[] SellOrders { get; set; }

    public override string ToString() => $"{Pair}Diff: {Nonce}: {BuyOrders.Length} buy orders and {SellOrders.Length} sell orders";
}
