namespace SpaceBattle
{
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
            ExecuteWithRetry(_inner, 0);
        }

        private void ExecuteWithRetry(ICommand cmd, int currentAttempt)
        {
            try
            {
                cmd.Execute();
            }
            catch
            {
                if (currentAttempt < _maxRetries)
                {
                    ExecuteWithRetry(cmd, currentAttempt + 1);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
