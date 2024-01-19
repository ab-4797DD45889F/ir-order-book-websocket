
class OrderBookService {

    #ui = {
        logContainerId: '',
        buyOrdersContainerId: '',
        sellOrdersContainerId: ''
    };

    #wsConnection = null;
    #orderBook = null;
    #pair = null;
    #url = null;

    constructor({logContainerId, buyOrdersContainerId, sellOrdersContainerId}) {
        this.#ui.logContainerId = logContainerId;
        this.#ui.buyOrdersContainerId = buyOrdersContainerId;
        this.#ui.sellOrdersContainerId = sellOrdersContainerId;
    }

    // initializes independent reserve order book
    start(pair, url) {
        this.#pair = pair;
        this.#url = url;

        // we need to first subscribe for the updates and only then request the first version of the order book
        this.#subscribeForUpdates();
    }

    #print(message) {
        console.log(message);
        return;

        const divOutput= document.getElementById(this.#ui.logContainerId);

        if (!divOutput) {
            console.log("output couldn't be found");
            return;
        }

        const child = document.createElement('pre');
        child.innerText = message;
        divOutput.insertBefore(child, divOutput.firstChild);
    }

    // subscribing for order book updates
    #subscribeForUpdates(){

        const self = this;

        // this will now work until we get
        this.#wsConnection = new WebSocket(`${this.#url}?channel=${this.#pair}`);
        this.#wsConnection.onopen = function () {
            self.#print("websocket connection opened");
        }

        this.#wsConnection.onmessage = function (evt) {
            const data = evt.data;
            // self.#print(">> " + data);
            const asObject = JSON.parse(data);

            if (asObject.t == 'OrderBook') {
                self.#print(`order book ${asObject.n}`);
                self.#orderBook = asObject;
                self.#showOrderBook();
            } else if (asObject.t == 'Update'){
                self.#print(`update ${asObject.n}`);
                self.#orderBook = self.#iterateOrderBook(self.#orderBook, asObject);
                self.#showOrderBook();
            } else {
                self.#print('unknown type' + asObject.t);
            }
        }

        this.#wsConnection.onclose = function () {
            self.#print("websocket connection closed");
            self.#wsConnection = null;
        }
    }

    #iterateOrderBook(left, update) {

        // todo: step update should be exactly 1

        if (!left || !update) {
            throw new Error("Both left and difference must be not null");
        }

        if (left.c !== update.c) {
            throw new Error(
                "Primary and Secondary currency codes do not match for the left order book and difference"
            );
        }

        return {
            // channel
            c: left.c,

            // index, nonce, timestamp
            n: update.n,

            // buy orders
            b: this.#iterateOrderCollection(left.b, update.b, true),

            // sell orders
            s: this.#iterateOrderCollection(left.s, update.s, false)
        };
    }

    #iterateOrderCollection(leftOrders, diffOrders, sortDescending) {
        const leftDict = {};
        leftOrders.forEach((order) => {
            leftDict[order.p] = order.v;
        });

        const diffDict = {};
        diffOrders.forEach((order) => {
            diffDict[order.p] = order.v;
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

            if(rightVolume > 0.000000001)
                right[price] = rightVolume;
        });

        let result = Object.keys(right).map((price) => ({
            p: parseFloat(price),
            v: right[price],
        }));

        if (sortDescending){
            result.sort( (a, b) => b.p - a.p);
        } else {
            result.sort( (a, b) => a.p - b.p);
        }

        return result;
    }

    #showOrderBook() {
        this.#showOrderCollection(this.#ui.buyOrdersContainerId, this.#orderBook.b, true);
        this.#showOrderCollection(this.#ui.sellOrdersContainerId, this.#orderBook.s, false);
    }

    #showOrderCollection(containerIdentifier, orders, volumeFirst) {
        const element = document.getElementById(containerIdentifier);
        if (element == null){
            this.#print(`element ${containerIdentifier} not found, can't show orders`);
            return;
        }

        // clearing previous data
        element.innerHTML = '';

        orders.forEach(order => {
            const orderElement = document.createElement('div');
            orderElement.textContent = volumeFirst
                ? `${order.v.toFixed(8)} - $${order.p.toFixed(2)}`
                : `$${order.p.toFixed(2)} - ${order.v.toFixed(8)}`;

            element.appendChild(orderElement);
        });

    }
}

// todo: show only last five records
// todo: show order book on the page (probably use vue.js)
