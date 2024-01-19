# ir-order-book-websocket
This is a pet project that tracks order book and broadcasts changes every second

## Sample output

```
Press Enter to quit...

XbtAudDiff :: {"Nonce":1705210000,"BuyOrders":[{"Price":63558.98,"Volume":0.114}],"SellOrders":[{"Price":64648.95,"Volume":0.196},{"Price":64648.96,"Volume":-0.196}]}

XbtAudDiff :: {"Nonce":1705210001,"BuyOrders":[{"Price":63558.98,"Volume":-0.114}],"SellOrders":[{"Price":64648.94,"Volume":0.196},{"Price":64648.95,"Volume":-0.196},{"Price":65027.62,"Volume":-0.062},{"Price":65040.01,"Volume":0.062},{"Price":65059.92,"Volume":-0.00071994}]}

XbtAudDiff :: {"Nonce":1705210002,"BuyOrders":[{"Price":63558.83,"Volume":0.114},{"Price":64235.31,"Volume":0.1}],"SellOrders":[{"Price":64309.38,"Volume":0.3},{"Price":64309.39,"Volume":-0.01582678},{"Price":64335.69,"Volume":-0.28468958},{"Price":65059.92,"Volume":0.00071994}]}

XbtAudDiff :: {"Nonce":1705210003,"BuyOrders":[{"Price":63558.83,"Volume":-0.114}],"SellOrders":[{"Price":65040.01,"Volume":-0.062},{"Price":65104.84,"Volume":0.062}]}

XbtAudDiff :: {"Nonce":1705210004,"BuyOrders":[{"Price":63558.98,"Volume":0.114}],"SellOrders":[{"Price":65040.01,"Volume":0.062},{"Price":65104.84,"Volume":-0.062}]}

```

## Runnninng in docker

```
docker build -t irorderbookwebapplication .
docker run -d -p 8080:8080 -p 8081:8081 --name irorderbook irorderbookwebapplication
```

todo: support 8081 as ssh port, it doesn't listen to https on 8081 port 
