// todo: use vuejs for UI

// define base url & port automatically
let wsProtocol = window.location.protocol === 'https:' ? 'wss' : 'ws';
let wsUrl = `${wsProtocol}://${window.location.host}/ws`;

// creating presentation layer
const orderBookUiModel = new OrderBookUiModel({
    logContainerId: 'output',
    buyOrdersContainerId: 'buyOrders',
    sellOrdersContainerId: 'sellOrders'
})

// creating order book service that will track order book changes
const orderBookTrackingService = new OrderBookTrackingService();
orderBookTrackingService.subscribe(function (orderBook) {
    orderBookUiModel.orderBookUpdated(orderBook);
});
orderBookTrackingService.start('XbtAud', wsUrl);
