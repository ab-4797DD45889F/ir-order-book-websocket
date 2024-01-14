using System.Net;
using System.Net.WebSockets;
using System.Text;

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

app.Map("/ws", async context =>
{
    Console.WriteLine("Some websocket related event registered");

    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();

        while (true)
        {
            var message = $"The current time is {DateTimeOffset.Now}";
            var bytes = Encoding.UTF8.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(arraySegment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
            {
                break;
            }

            Thread.Sleep(1000);
        }
        Console.WriteLine("ws client disconnected");
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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


await app.RunAsync();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
