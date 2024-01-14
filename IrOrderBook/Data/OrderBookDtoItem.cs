using System.Text.Json.Serialization;

namespace IrOrderBook.Data;

/// <summary>
/// A lightweight order book item representing how many volume available in the order book at a specific price.
/// </summary>
public class OrderBookDtoItem
{
    [JsonPropertyName("p")]
    public decimal Price { get; set; }

    [JsonPropertyName("v")]
    public decimal Volume { get; set; }
}
