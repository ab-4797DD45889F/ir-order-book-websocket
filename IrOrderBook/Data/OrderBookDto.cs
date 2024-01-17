using System.Text.Json.Serialization;

namespace IrOrderBook.Data;

/// <summary>
/// Short version of the order book that we receive from the API.
/// Buy and sell orders collections are presented as a lightweight objects containing price and volume only, excluding the order type which in fact doesn't make sense.
/// </summary>
public class OrderBookDto
{
    /// <summary>
    /// The version of the order book.
    /// </summary>
    [JsonPropertyName("n")]
    public long Nonce { get; set; }

    /// <summary>
    /// Currency Pair of the order book.
    /// </summary>
    [JsonPropertyName("c")]
    public string Pair { get; set; }

    /// <summary>
    /// The list of available buy orders.
    /// </summary>
    [JsonPropertyName("b")]
    public OrderBookDtoItem[] BuyOrders { get; set; }

    /// <summary>
    /// The list of available sell orders.
    /// </summary>
    [JsonPropertyName("s")]
    public OrderBookDtoItem[] SellOrders { get; set; }
}
