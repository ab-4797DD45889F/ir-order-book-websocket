
// initializes independent reserve order book
function init() {
    // we need to first subscribe for the updates and only then request the first version of the order book
    subscribeForUpdates();
    getOrderBook();
}

// subscribing for order book updates
function subscribeForUpdates(){
    // this will now work until we get
    const ws = new WebSocket("wss://localhost:7253/ws");
    ws.onopen = function () {
        console.log("websocket connection opened");
    }

    ws.onmessage = function (evt) {
        const orderBookUpdate = evt.data;
        console.log("update received >>", orderBookUpdate);
    }

    ws.onclose = function () {
        console.log("websocket connection closed");
    }
}

// getting initial version of the order book
function getOrderBook(){
    console.log("todo: get the first version of the order book")
}

console.log("hello world from the ir-order-book.js");

init();
