using System.Net;
using IndependentReserve.DotNetClientApi.Data;
using IrOrderBook.Extensions;
using IrOrderBook.Interfaces;
using IrOrderBook.Services;
using IrOrderBookWebApplication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// todo: first implement rough working solution as a sample application and then implement more robust and mature solution

// todo: initiate order book
// todo: add controller (or simple map route) to return order book
// todo: implement all methods for xbt/aud
// todo: read more on how websockets work, maintain websockets better, broadcast order book updates
// todo: doesn't seem we get event when the client disconnects
// todo: add some framework like vuejs and layout the order book, add some animation for when rows are added or deleted

// todo: probably rework websockets towards SignalR

var app = builder.Build();

// todo: limit allowed origins to ir.com
app.UseWebSockets();

const CurrencyCode primary = CurrencyCode.Xbt;
const CurrencyCode secondary = CurrencyCode.Aud;
var cts = new CancellationTokenSource();
var cancellationToken = cts.Token;
var clientConnectionService = new ClientConnectionService(cancellationToken);

var trackingService = new SingleOrderBookTrackingService(primary, secondary, (IBroadcastService)clientConnectionService);

var orderBookReady = new ManualResetEvent(false);

Task.Run(async () =>
{
    await trackingService.Start(cancellationToken);
    orderBookReady.Set();
});

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        // we need to wait the order book service to get the first version of the order book, otherwise we will return null to the first client
        orderBookReady.WaitOne();

        // if we support more order books - we will get necessary order book service by channel name which is XbtAud for example
        await clientConnectionService.Register(context, (channelName) => trackingService.GetOrderBook().ToJson());
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

// todo: add an endpoint that would return order book
// todo: add an endpoint to get stats
// todo: next step is build SPA, that would let choose market from the dropdown

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

await app.RunAsync(cancellationToken);

Console.WriteLine("app seems to have been completed, setting the cancellation token");

// todo: this doesn't work, what is the correct way to stop the web application ?

cts.Cancel();
