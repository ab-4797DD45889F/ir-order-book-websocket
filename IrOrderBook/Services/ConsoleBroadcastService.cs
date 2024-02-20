using IrOrderBook.Interfaces;

namespace IrOrderBook.Services;

/// <summary>
/// For test/debug purposes we can launch the application as a console app that will write the order book differences into the console.
/// </summary>
public class ConsoleBroadcastService : IBroadcastService
{
    public async Task Broadcast(string channelName, string message)
    {
        Console.WriteLine($"{channelName} :: {message}\n");
    }
}
