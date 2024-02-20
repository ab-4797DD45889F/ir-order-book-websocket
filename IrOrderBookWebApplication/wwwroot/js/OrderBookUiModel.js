class OrderBookUiModel {

    #ui = {
        logContainerId: '',
        buyOrdersContainerId: '',
        sellOrdersContainerId: ''
    };

    constructor({logContainerId, buyOrdersContainerId, sellOrdersContainerId}) {
        this.#ui.logContainerId = logContainerId;
        this.#ui.buyOrdersContainerId = buyOrdersContainerId;
        this.#ui.sellOrdersContainerId = sellOrdersContainerId;
    }

    orderBookUpdated(orderBook) {
        this.#showOrderCollection(this.#ui.buyOrdersContainerId, orderBook.b, true);
        this.#showOrderCollection(this.#ui.sellOrdersContainerId, orderBook.s, false);
    }

    #numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    #showOrderCollection(containerIdentifier, orders, volumeFirst) {
        const element = document.getElementById(containerIdentifier);
        if (element == null){
            return;
        }

        // clearing previous data
        element.innerHTML = '';

        orders.forEach(order => {
            const volumeString = order.v.toFixed(8);
            const priceString = this.#numberWithCommas(order.p.toFixed(2));
            const orderElement = document.createElement('div');
            orderElement.innerHTML = volumeFirst
                ? `${volumeString} <span class="buy">$${priceString}</span>`
                : `<span class="sell">$${priceString}</span> ${volumeString}`;

            element.appendChild(orderElement);
        });
    }
}
