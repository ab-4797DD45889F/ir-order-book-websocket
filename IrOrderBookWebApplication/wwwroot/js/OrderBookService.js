// todo: don't use JS float because they don't guarantee precision
class OrderBookService {

    iterateOrderBook(left, update) {

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
}
