# ir-order-book-websocket
This is a pet project that tracks order book and broadcasts changes every second

## TLDR

```
$ docker pull ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest
$ docker run -d -p 8080:8080 -p 8081:8081 --name irorderbook ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest
```

## Run in docker

```
$ docker build -t ir-order-book-websocket .
$ docker run -d -p 8080:8080 -p 8081:8081 --name irorderbook ir-order-book-websocket
```

## Publish docker image

Personal access token (PAT) with `write:packages` and `read:packages` privileges is required.

```
echo TOKEN | docker login ghcr.io -u USERNAME --password-stdin 
```

```
$ echo TOKEN | docker login ghcr.io -u USERNAME --password-stdin 
$ git clean -fXd
$ docker build -t ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest .
$ docker push ghcr.io/ab-4797dd45889f/ir-order-book-websocket:latest
```
