
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
    const ws = new WebSocket("wss://localhost:7253/ws?channel=XbtAud");
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

// todo: build order book
// todo: show order book on the page (probably use vue.js)

function ReconstructRightWithDifference(left, difference) {
    if (!left || !difference) {
        throw new Error("Both left and difference must be not null");
    }

    if (left.Pair !== difference.Pair) {
        throw new Error(
            "Primary and Secondary currency codes do not match for the left order book and difference"
        );
    }

    return {
        Pair: left.Pair,
        BuyOrders: ReconstructCollectionWithDifference(
            left.BuyOrders,
            difference.BuyOrders
        ),
        SellOrders: ReconstructCollectionWithDifference(
            left.SellOrders,
            difference.SellOrders
        ),
    };
}

function ReconstructCollectionWithDifference(leftOrders, diffOrders) {
    const leftDict = {};
    leftOrders.forEach((order) => {
        leftDict[order.Price] = order.Volume;
    });

    const diffDict = {};
    diffOrders.forEach((order) => {
        diffDict[order.Price] = order.Volume;
    });

    const right = {};

    const allPrices = new Set([
        ...Object.keys(leftDict).map((p) => parseFloat(p)),
        ...Object.keys(diffDict).map((p) => parseFloat(p)),
    ]);

    allPrices.forEach((price) => {
        const leftVolume = leftDict[price] || 0;
        const diff = diffDict[price] || 0;

        const rightVolume = diff + leftVolume;

        if(rightVolume > 0)
            right[price] = rightVolume;
    });

    return Object.keys(right).map((price) => ({
        Price: parseFloat(price),
        Volume: right[price],
    }));
}

console.log("hello world from the ir-order-book.js");

// todo: init in a second to give backend time to init the first version of the order book
window.setTimeout(() => { init(); }, 5000);
// init();
