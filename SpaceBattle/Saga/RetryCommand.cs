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
            for (int attempt = 0; attempt <= _maxRetries; attempt++)
            {
                try
                {
                    _inner.Execute();
                    return; // Успешно — выходим
                }
                catch when (attempt < _maxRetries)
                {
                    // Игнорируем, пробуем снова
                }
            }

            // Все попытки исчерпаны — пробрасываем исключение
            throw new Exception($"Command failed after {_maxRetries + 1} attempts.");
        }
    }
}
