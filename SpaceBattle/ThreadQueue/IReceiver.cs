namespace SpaceBattle;
public interface IReceiver
{
    ICommand Receive();
    bool isEmpty();
}
