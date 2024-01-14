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
    public long Nonce { get; set; }

    /// <summary>
    /// Primary currency code of the order book.
    /// </summary>
    public string Primary { get; set; }

    /// <summary>
    /// Secondary currency code of the order book.
    /// </summary>
    public string Secondary { get; set; }

    /// <summary>
    /// The list of available buy orders.
    /// </summary>
    public OrderBookDtoItem[] BuyOrders { get; set; }

    /// <summary>
    /// The list of available sell orders.
    /// </summary>
    public OrderBookDtoItem[] SellOrders { get; set; }
}