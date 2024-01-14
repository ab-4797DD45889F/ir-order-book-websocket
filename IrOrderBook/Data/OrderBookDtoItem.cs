namespace IrOrderBook.Data;

/// <summary>
/// A lightweight order book item representing how many volume available in the order book at a specific price.
/// </summary>
public class OrderBookDtoItem
{
    public decimal Price { get; set; }
    public decimal Volume { get; set; }
}