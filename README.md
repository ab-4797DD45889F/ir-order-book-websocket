# ir-order-book-websocket
This is a pet project that tracks order book and broadcasts changes every second

## Sample output

```
XbtAud: 1705073252: 748 buy orders and 326 sell orders
DiffXbtAud: 1705073252: 11 buy orders and 24 sell orders
{"Index":1705073252,"Primary":1,"Secondary":3,"BuyOrders":[{"Price":66204.1,"Volume":1.927},{"Price":66309.3,"Volume":-1.927},{"Price":66705.47,"Volume":-0.22},{"Price":66712.15,"Volume":0.21},{"Price":66712.
16,"Volume":0.14815242},{"Price":66870.0,"Volume":1.0},{"Price":66870.02,"Volume":-0.1228039},{"Price":66885.0,"Volume":-1.0},{"Price":66900.03,"Volume":0.3},{"Price":66915.0,"Volume":-1.0},{"Price":66915.01,
"Volume":-0.3491}],"SellOrders":[{"Price":67055.0,"Volume":1.0},{"Price":67065.0,"Volume":-1.0},{"Price":67065.01,"Volume":0.27854193},{"Price":67084.99,"Volume":-0.27854193},{"Price":67085.0,"Volume":1.0},{"
Price":67095.0,"Volume":-1.0},{"Price":67173.13,"Volume":0.05966477},{"Price":67179.84,"Volume":-0.13647247},{"Price":67179.85,"Volume":-0.05967021},{"Price":67194.46,"Volume":0.16374734},{"Price":67252.64,"V
olume":-0.15872918},{"Price":67493.38,"Volume":0.047},{"Price":67493.4,"Volume":-0.047},{"Price":67503.23,"Volume":0.126},{"Price":67503.24,"Volume":0.16252555},{"Price":67523.37,"Volume":-0.126},{"Price":675
23.39,"Volume":-0.18487689},{"Price":67645.94,"Volume":0.134},{"Price":67815.69,"Volume":-0.062},{"Price":67844.13,"Volume":-0.062},{"Price":67874.03,"Volume":0.062},{"Price":68001.23,"Volume":0.046},{"Price":68023.37,"Volume":0.001},{"Price":68138.82,"Volume":-0.043}]}

XbtAud: 1705073253: 748 buy orders and 326 sell orders
DiffXbtAud: 1705073253: 16 buy orders and 30 sell orders
{"Index":1705073253,"Primary":1,"Secondary":3,"BuyOrders":[{"Price":65966.39,"Volume":-0.026},{"Price":66034.25,"Volume":0.025},{"Price":66414.91,"Volume":-0.21574891},{"Price":66414.95,"Volume":0.21216824},{
"Price":66463.23,"Volume":0.051},{"Price":66464.94,"Volume":-0.051},{"Price":66601.41,"Volume":-0.05999872},{"Price":66604.05,"Volume":0.05997208},{"Price":66712.14,"Volume":-0.05986213},{"Price":66712.15,"Vo
lume":-0.21},{"Price":66712.16,"Volume":-0.14815242},{"Price":66718.82,"Volume":0.05985816},{"Price":66891.43,"Volume":0.13264656},{"Price":66900.03,"Volume":-0.3},{"Price":66900.08,"Volume":0.3},{"Price":669
00.11,"Volume":0.3491}],"SellOrders":[{"Price":67030.16,"Volume":-0.03},{"Price":67030.17,"Volume":-0.12682782},{"Price":67055.0,"Volume":-1.0},{"Price":67059.2,"Volume":0.03},{"Price":67059.21,"Volume":0.126
76871},{"Price":67065.01,"Volume":-0.27854193},{"Price":67069.99,"Volume":0.27854193},{"Price":67070.0,"Volume":1.0},{"Price":67085.0,"Volume":-1.0},{"Price":67100.0,"Volume":1.0},{"Price":67114.99,"Volume":0
.13781388},{"Price":67166.41,"Volume":0.05966625},{"Price":67173.13,"Volume":-0.05966477},{"Price":67194.46,"Volume":-0.16374734},{"Price":67197.83,"Volume":-0.05958526},{"Price":67245.04,"Volume":0.0595588},
{"Price":67473.26,"Volume":0.17578139},{"Price":67493.37,"Volume":0.047},{"Price":67493.38,"Volume":-0.047},{"Price":67503.22,"Volume":0.123},{"Price":67503.23,"Volume":-0.126},{"Price":67503.24,"Volume":-0.1
6252555},{"Price":67645.94,"Volume":-0.134},{"Price":67673.32,"Volume":0.134},{"Price":67800.68,"Volume":0.062},{"Price":67867.26,"Volume":0.062},{"Price":67874.03,"Volume":-0.062},{"Price":68001.22,"Volume":0.046},{"Price":68001.23,"Volume":-0.046},{"Price":68023.37,"Volume":-0.001}]}

```
