# ir-order-book-websocket

https://ir-order-book.8bits.kg/

This is a pet project that tracks order book and broadcasts changes every second in the following format so that the subscribed clients could calculate the current state of the order book based on the small differences

```
{"t":"OrderBook","n":1707190312,"c":"XbtAud","b":[{"p":65823.4,"v":0.33},{"p":65796.45,"v":0.06071961},{"p":65789.88,"v":0.65000000}, ... ],"s":[{"p":65874.95,"v":0.32672583},{"p":65875.0,"v":1.0}, ... ]}
{"t":"Update","n":1707190313,"c":"XbtAud","b":[{"p":65113.9,"v":-0.051},{"p":65147.89,"v":-0.063}, ... ],"s":[{"p":65863.23,"v":0.26991634},{"p":65863.4,"v":0.12427004}, ... ]}
{"t":"Update","n":1707190314,"c":"XbtAud","b":[{"p":65147.97,"v":-0.062},{"p":65222.37,"v":-0.114}, ... ],"s":[{"p":65868.26,"v":0.1},{"p":65868.27,"v":0.14911809}, ... ]}
{"t":"Update","n":1707190315,"c":"XbtAud","b":[],"s":[]}
```

## Contribute

You can easily take part in the development and improvement of this project yourself with VS code and devcontaners.

```
$ git clone https://github.com/ab-4797DD45889F/ir-order-book-websocket.git
$ cd ir-order-book-websocket
$ code . 
```

- Assuming you already use VS Code, install `Dev Containers` extension
- Use the Command Palette (Ctrl + Shift + P) and choose `Dev Containers: Rebuild and Reopen in container`
- Once the project gets reload in container, open the vscode terminal and run the following commands to start the application

```
$ dotnet restore
$ cd ./IrOrderBookWebApplication
$ dotnet run
```

Live order book should be availabe by the url http://localhost:5282

## Run locally

```
$ docker pull ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest
$ docker run -d -p 8080:8080 -p 8081:8081 --name irorderbook ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest
```

## Build sources and run in docker

```
$ docker build -t ir-order-book-websocket .
$ docker run -d -p 8080:8080 -p 8081:8081 --name irorderbook ir-order-book-websocket
```

## Publish docker image

Personal access token (PAT) with `write:packages` and `read:packages` privileges is required.

```
$ echo TOKEN | docker login ghcr.io -u USERNAME --password-stdin 
```

```
$ git clean -fXd
$ docker build -t ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest .
$ docker push ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest
```
