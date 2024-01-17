using IrOrderBook.Interfaces;

namespace IrOrderBook.Services;

public class ConsoleBroadcastService : IBroadcastService
{
    public async Task Broadcast(string channelName, string message)
    {
        Console.WriteLine($"{channelName} :: {message}\n");
    }
}
