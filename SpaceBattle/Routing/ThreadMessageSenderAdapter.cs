using Hwdtech;


namespace SpaceBattle;
using System.Collections.Concurrent;

public class ThreadMessageSenderAdapter : ISender
{
    BlockingCollection<ICommand> queue;

    public ThreadMessageSenderAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public void Send(object message)
    {
        queue.Add(new InterpretThreadMessageCommand((IMessage) message));
    }
}