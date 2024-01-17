namespace IrOrderBook.Interfaces;

public interface IBroadcastService
{
    Task Broadcast(string channelName, string message);
}
