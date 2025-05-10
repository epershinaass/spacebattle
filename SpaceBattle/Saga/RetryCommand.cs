namespace SpaceBattle;
public class RetryCommand : ICommand
{
    private readonly ICommand _inner;
    private readonly int _maxRetries;

    public RetryCommand(ICommand inner, int maxRetries)
    {
        _inner = inner;
        _maxRetries = maxRetries;
    }
    public void Execute()
    {
        int attempt = 0;
        while (true)
        {
            try
            {
                _inner.Execute();
                return;
            }
            catch
            {
                attempt++;
                if (attempt > _maxRetries)
                    throw;
            }
        }
    }
}