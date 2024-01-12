namespace IrOrderBook.Interfaces;

public interface IBroadcastService
{
    void Broadcast(string channelName, string message);
}
