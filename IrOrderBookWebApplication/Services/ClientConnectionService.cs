using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using IrOrderBook.Interfaces;

namespace IrOrderBookWebApplication.Services;

public class ClientConnectionService : IBroadcastService
{
    private readonly CancellationToken _cancellationToken;

    /// <summary>
    /// Active clients grouped by channel name
    /// </summary>
    private readonly ConcurrentDictionary<string, ConcurrentBag<WebSocket>> _wsConnections = new();

    public ClientConnectionService(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public async Task Register(HttpContext context, Func<string, string> getResponse)
    {
        var channelName = context.Request.Query["channel"];

        // todo: argument validation XbtAudDiff

        Console.WriteLine("Some websocket related event registered");

        using var socket = await context.WebSockets.AcceptWebSocketAsync();
        var socketFinishedTcs = new TaskCompletionSource<object>(); // todo: still not sure about this

        var connectionsByChannel = _wsConnections.GetOrAdd(channelName, _ => new ConcurrentBag<WebSocket>());
        connectionsByChannel.Add(socket);

        // sending current version of the order book in response
        var response = getResponse(channelName);
        var arraySegment = ToArraySegment(response);
        await SendToSocket(socket, arraySegment);

        await socketFinishedTcs.Task;
    }

    public async Task Broadcast(string channelName, string message)
    {
        var arraySegment = ToArraySegment(message);
        var connectionsByChannel = _wsConnections.GetOrAdd(channelName, _ => new ConcurrentBag<WebSocket>()).ToArray(); // converting to array so that removing inactive clients will not cause problems

        Console.WriteLine($"{channelName} ({connectionsByChannel.Length} clients) :: {message}\n");

        foreach (var socket in connectionsByChannel)
        {
            if (socket.State == WebSocketState.Open)
            {
                try
                {
                    await SendToSocket(socket, arraySegment);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception while sending message to connected client {ex.Message}");
                }
            }
        }
    }

    private ArraySegment<byte> ToArraySegment(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
        return arraySegment;
    }

    private Task SendToSocket(WebSocket socket, ArraySegment<byte> arraySegment)
    {
        return socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, _cancellationToken);
    }
}
