
const orderBookService = new OrderBookService({
    logContainerId: 'output',
    buyOrdersContainerId: 'buyOrders',
    sellOrdersContainerId: 'sellOrders'
});

// todo: define base url & port automatically

//orderBookService.start('XbtAud', 'wss://localhost:7253/ws');
orderBookService.start('XbtAud', 'ws://127.0.0.1:8080/ws');
