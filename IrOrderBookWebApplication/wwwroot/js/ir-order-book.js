
// initializes independent reserve order book
function init() {
    // we need to first subscribe for the updates and only then request the first version of the order book
    subscribeForUpdates();
    getOrderBook();
}

function print(message) {
    console.log(message);

    const divOutput= document.getElementById('output');

    if (!divOutput) {
        console.log("output couldn't be found");
        return;
    }

    const child = document.createElement('pre');
    child.innerText = message;
    divOutput.insertBefore(child, divOutput.firstChild);
}

// subscribing for order book updates
function subscribeForUpdates(){
    // this will now work until we get
    const ws = new WebSocket("wss://localhost:7253/ws?name=ab");
    ws.onopen = function () {
        print("websocket connection opened");
    }

    ws.onmessage = function (evt) {
        const orderBookUpdate = evt.data;
        print("update received >>" + orderBookUpdate);
    }

    ws.onclose = function () {
        print("websocket connection closed");
    }
}

// getting initial version of the order book
function getOrderBook(){
    console.log("todo: get the first version of the order book")
}

console.log("hello world from the ir-order-book.js");

init();
