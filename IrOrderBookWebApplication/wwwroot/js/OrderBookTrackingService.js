// todo: step update should be exactly 1 otherwise need to re-connect again
// todo: try to reconnect when disconnected

class OrderBookTrackingService {

    #wsConnection = null;
    #orderBook = null;
    #pair = null;
    #url = null;
    #callback = null;

    #orderBookService = new OrderBookService();

    // initializes independent reserve order book
    start(pair, url) {
        this.#pair = pair;
        this.#url = url;

        // we need to first subscribe for the updates and only then request the first version of the order book
        this.#subscribeForUpdates();
    }

    // subscribes
    subscribe(callback) {
        this.#callback = callback;
    }

    #print(message) {
        console.log(message);
    }

    // subscribing for order book updates
    #subscribeForUpdates() {

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

            if (asObject.t === 'OrderBook') {
                self.#print(`order book ${asObject.n}`);
                self.#orderBook = asObject;
                if (self.#callback != null) {
                    self.#callback(self.#orderBook);
                }
            } else if (asObject.t === 'Update') {
                self.#print(`update ${asObject.n}`);
                self.#orderBook = self.#orderBookService.iterateOrderBook(self.#orderBook, asObject);
                if (self.#callback != null) {
                    self.#callback(self.#orderBook);
                }
            } else {
                self.#print('unknown type' + asObject.t);
            }
        }

        this.#wsConnection.onclose = function () {
            self.#print("websocket connection closed");
            self.#wsConnection = null;
        }
    }
}
