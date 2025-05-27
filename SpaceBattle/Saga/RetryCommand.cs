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

        private void ExecuteWithRetry(ICommand _inner, int currentAttempt)
        {
            try
            {
                _inner.Execute();
            }
            catch
            {
                if (currentAttempt < _maxRetries)
                {
                    ExecuteWithRetry(_inner, currentAttempt + 1);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
