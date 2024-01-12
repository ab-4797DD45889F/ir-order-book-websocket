using IrOrderBook.Interfaces;

namespace IrOrderBook.Services;

public class ConsoleBroadcastService : IBroadcastService
{
    public void Broadcast(string channelName, string message)
    {
        Console.WriteLine($"{channelName} :: {message}\n");
    }
}
