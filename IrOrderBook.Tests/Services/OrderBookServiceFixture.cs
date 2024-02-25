using IrOrderBook.Data;
using IrOrderBook.Services;
using NUnit.Framework;

namespace IrOrderBook.Tests.Services;

[TestFixture]
public class OrderBookServiceFixture
{
    private OrderBookService _orderBookService;

    // todo: make strict order of the collections, see what api returns and make the order the same

    [SetUp]
    public void Setup()
    {
        _orderBookService = new OrderBookService();
    }

    [Test]
    public void GetDifference_WhenLeftOrRightIsNull_ThrowsArgumentException()
    {
        // Arrange
        OrderBookDto orderBookNull = null;
        var orderBookNotNull = new OrderBookDto();

        // Act & Assert
        {
            var ex = Assert.Throws<ArgumentException>(() => _orderBookService.GetDifference(orderBookNull, orderBookNotNull));
            Assert.IsNotNull(ex);
            Assert.That(ex.Message, Is.EqualTo("Both left and right must be not null"));
        }

        {
            var ex = Assert.Throws<ArgumentException>(() => _orderBookService.GetDifference(orderBookNotNull, orderBookNull));
            Assert.IsNotNull(ex);
            Assert.That(ex.Message, Is.EqualTo("Both left and right must be not null"));
        }
    }

    [Test]
    public void GetDifference_WhenPrimaryAndSecondaryCurrencyCodesDoNotMatch_ThrowsArgumentException()
    {
        // Arrange
        var left = new OrderBookDto { Pair = "XbtAud" };
        var right = new OrderBookDto { Pair = "EthUsd" };

        // Act & Assert
        {
            var ex = Assert.Throws<ArgumentException>(() => _orderBookService.GetDifference(left, right));
            Assert.IsNotNull(ex);
            Assert.That(ex.Message, Is.EqualTo("Primary and Secondary currency codes do not match for the left and right order books"));
        }
    }

    /// <summary>
    /// A test to make sure that the service calculates valid difference.
    /// </summary>
    [Test]
    public void GetDifference()
    {
        // Arrange
        var left = new OrderBookDto
        {
            Nonce = 100
            , Pair = "XbtAud"
            , BuyOrders = new[] { new OrderBookDtoItem(78_000m, 1m), new OrderBookDtoItem(77_000m, 3m) }
            , SellOrders = new[] { new OrderBookDtoItem(79_000m, 1m), new OrderBookDtoItem(80_000m, 3m) }
        };

        var right = new OrderBookDto
        {
            Nonce = 101
            , Pair = "XbtAud"
            , BuyOrders = new[] { new OrderBookDtoItem(79_000m, 1m), new OrderBookDtoItem(78_000m, 3m) }
            , SellOrders = new[] { new OrderBookDtoItem(80_000m, 1m), new OrderBookDtoItem(81_000m, 3m) }
        };

        var expectedDifference = new OrderBookDifference()
        {
            Nonce = 101
            , Pair = "XbtAud"
            , BuyOrders = new []
            {
                new OrderBookDtoItem(77_000m, -3m)
                , new OrderBookDtoItem(78_000m, +2m)
                , new OrderBookDtoItem(79_000m, +1m)
            }
            , SellOrders = new []
            {
                new OrderBookDtoItem(79_000m, -1m)
                , new OrderBookDtoItem(80_000m, -2m)
                , new OrderBookDtoItem(81_000m, +3m)
            }
        };

        // Act & Assert
        {
            var actualDifference = _orderBookService.GetDifference(left, right);

            ExpectDifference(expectedDifference, actualDifference);
        }
    }

    [Test]
    public void ApplyDifference_WhenLeftOrDifferenceIsNull_ThrowsArgumentException()
    {
        // Act & Assert
        {
            var ex = Assert.Throws<ArgumentException>(() => _orderBookService.ApplyDifference(new OrderBookDto(), null));
            Assert.IsNotNull(ex);
            Assert.That(ex.Message, Is.EqualTo("Both left order book and difference must be not null"));
        }

        {
            var ex = Assert.Throws<ArgumentException>(() => _orderBookService.ApplyDifference(null, new OrderBookDifference()));
            Assert.IsNotNull(ex);
            Assert.That(ex.Message, Is.EqualTo("Both left order book and difference must be not null"));
        }
    }

    [Test]
    public void ApplyDifference_WhenPrimaryAndSecondaryCurrencyCodesDoNotMatch_ThrowsArgumentException()
    {
        // Arrange
        var left = new OrderBookDto { Pair = "XbtAud" };
        var difference = new OrderBookDifference { Pair = "EthUsd" };

        // Act & Assert
        {
            var ex = Assert.Throws<ArgumentException>(() => _orderBookService.ApplyDifference(left, difference));
            Assert.IsNotNull(ex);
            Assert.That(ex.Message, Is.EqualTo("Primary and Secondary currency codes do not match for the left order book and difference"));
        }
    }

    /// <summary>
    /// A test to make sure that the service calculate valid order book based on the left version and the difference.
    /// </summary>
    [Test]
    public void ApplyOrderBook()
    {
        // Arrange
        var left = new OrderBookDto
        {
            Nonce = 100
            , Pair = "XbtAud"
            , BuyOrders = new[] { new OrderBookDtoItem(78_000m, 1m), new OrderBookDtoItem(77_000m, 3m) }
            , SellOrders = new[] { new OrderBookDtoItem(79_000m, 1m), new OrderBookDtoItem(80_000m, 3m) }
        };

        var difference = new OrderBookDifference()
        {
            Nonce = 101
            , Pair = "XbtAud"
            , BuyOrders = new []
            {
                new OrderBookDtoItem(77_000m, -3m)
                , new OrderBookDtoItem(78_000m, +2m)
                , new OrderBookDtoItem(79_000m, +1m)
            }
            , SellOrders = new []
            {
                new OrderBookDtoItem(79_000m, -1m)
                , new OrderBookDtoItem(80_000m, -2m)
                , new OrderBookDtoItem(81_000m, +3m)
            }
        };

        var expectedRight = new OrderBookDto
        {
            Nonce = 101
            , Pair = "XbtAud"
            , BuyOrders = new[] { new OrderBookDtoItem(78_000m, 3m), new OrderBookDtoItem(79_000m, 1m) }
            , SellOrders = new[] { new OrderBookDtoItem(80_000m, 1m), new OrderBookDtoItem(81_000m, 3m) }
        };

        // Act & Assert
        {
            var actualRight = _orderBookService.ApplyDifference(left, difference);

            ExpectOrderBook(expectedRight, actualRight);
        }
    }


    private void ExpectDifference(OrderBookDifference expectedDifference, OrderBookDifference actualDifference)
    {
        Assert.IsNotNull(expectedDifference, "Expected difference should not be null");
        Assert.IsNotNull(actualDifference, "Actual difference should not be null");
        Assert.That(actualDifference.Pair, Is.EqualTo(expectedDifference.Pair), "Pair should match");
        Assert.That(actualDifference.Nonce, Is.EqualTo(expectedDifference.Nonce), "Nonce should match");

        Assert.IsNotNull(actualDifference.BuyOrders, "Buy orders difference should not be null");
        Assert.IsNotNull(actualDifference.SellOrders, "Sell orders difference should not be null");

        ExpectOrdersCollection(expectedDifference.BuyOrders, actualDifference.BuyOrders);
        ExpectOrdersCollection(expectedDifference.SellOrders, actualDifference.SellOrders);
    }

    private void ExpectOrdersCollection(OrderBookDtoItem[] expectedOrders, OrderBookDtoItem[] actualOrders)
    {
        Assert.That(actualOrders.Length, Is.EqualTo(expectedOrders.Length));

        for (var i = 0; i < expectedOrders.Length; i++)
        {
            Assert.That(actualOrders[i].Price, Is.EqualTo(expectedOrders[i].Price), $"Price at index {i} should match");
            Assert.That(actualOrders[i].Volume, Is.EqualTo(expectedOrders[i].Volume), $"Volume at index {i} should match");
        }
    }

    private void ExpectOrderBook(OrderBookDto expectedOrderBook, OrderBookDto actualOrderBook)
    {
        Assert.IsNotNull(expectedOrderBook, "Expected order book should not be null");
        Assert.IsNotNull(actualOrderBook, "Actual order book should not be null");
        Assert.That(actualOrderBook.Pair, Is.EqualTo(expectedOrderBook.Pair), "Pair should match");
        Assert.That(actualOrderBook.Nonce, Is.EqualTo(expectedOrderBook.Nonce), "Nonce should match");

        Assert.IsNotNull(actualOrderBook.BuyOrders, "Buy orders should not be null");
        Assert.IsNotNull(actualOrderBook.SellOrders, "Sell orders should not be null");

        ExpectOrdersCollection(expectedOrderBook.BuyOrders, actualOrderBook.BuyOrders);
        ExpectOrdersCollection(expectedOrderBook.SellOrders, actualOrderBook.SellOrders);    }
}
