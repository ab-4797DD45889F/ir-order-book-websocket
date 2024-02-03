// todo: representation level (UI) should not be a part of the order book
// todo: show only last five records
// todo: show order book on the page (probably use vue.js)

// creating order book service that will track order book changes

const orderBookService = new OrderBookService({
    logContainerId: 'output',
    buyOrdersContainerId: 'buyOrders',
    sellOrdersContainerId: 'sellOrders'
});

// define base url & port automatically

let wsProtocol = window.location.protocol === 'https:' ? 'wss' : 'ws';
let wsUrl = `${wsProtocol}://${window.location.host}/ws`;
orderBookService.start('XbtAud', wsUrl);
