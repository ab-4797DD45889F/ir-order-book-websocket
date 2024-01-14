using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Data;

namespace IrOrderBook.Extensions;

public static class OrderBookExtensions
{
    public static OrderBookDto ToDto(this OrderBookWrapper orderBookWrapper, long timestamp)
    {
        var orderBook = orderBookWrapper.Original;

        var dto = new OrderBookDto();
        dto.Nonce = timestamp;
        dto.Primary = orderBook.PrimaryCurrencyCode.ToString();
        dto.Secondary = orderBook.SecondaryCurrencyCode.ToString();
        dto.BuyOrders = orderBook.BuyOrders.Select(ToDto).ToArray();
        dto.SellOrders = orderBook.SellOrders.Select(ToDto).ToArray();
        return dto;
    }

    public static OrderBookDtoItem ToDto(this OrderBookItem item)
    {
        return new OrderBookDtoItem
        {
            Price = item.Price
            , Volume = item.Volume
        };
    }
}
