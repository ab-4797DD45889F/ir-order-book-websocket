using System.Net;
using System.Net.WebSockets;
using System.Text;

var cancellationToken = new CancellationToken();

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
// todo: remove forecast thing

var app = builder.Build();

app.UseWebSockets();

var wsConnections = new List<WebSocket>();

async Task Broadcast(string message)
{
    var bytes = Encoding.UTF8.GetBytes(message);
    var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);

    foreach (var socket in wsConnections)
    {
        if (socket.State == WebSocketState.Open)
        {
            await socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, cancellationToken);
        }
    }
}

/*
 *
         else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
        {
            break;
        }
*
 */

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var username = context.Request.Query["name"];

        Console.WriteLine("Some websocket related event registered");
        var ws = await context.WebSockets.AcceptWebSocketAsync();

        wsConnections.Add(ws);

        await Broadcast($"{username} jointed the room");
        await Broadcast($"{wsConnections.Count} users connected");
        await ws.ReceiveAsync(new ArraySegment<byte>(new byte[1024]), cancellationToken);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});


app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();


Task.Run(async () =>
{
    while (!cancellationToken.IsCancellationRequested)
    {
        var message = $"The current time is {DateTimeOffset.Now}";

        Console.WriteLine($"Sending message to {wsConnections.Count}");

        await Broadcast(message);

        Thread.Sleep(1000);
    }
});

await app.RunAsync(cancellationToken);

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
