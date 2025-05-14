namespace SpaceBattle;
public class RetryCommand : ICommand
{
    private readonly ICommand inner;
    private readonly int maxRetries;

    public RetryCommand(ICommand inner, int maxRetries)
    {
        this.inner = inner;
        this.maxRetries = maxRetries;
    }
    public void Execute()
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                inner.Execute();
                return;
            }
            catch{
                attempt++;
                if (attempt > maxRetries) throw;
            }
        }
    }
}