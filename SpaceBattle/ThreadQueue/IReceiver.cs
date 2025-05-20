namespace SpaceBattle;
interface IReceiver
{
    ICommand Receive();
    bool isEmpty();
}
