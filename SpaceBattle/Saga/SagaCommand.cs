using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceBattle
{
    public class SagaCommand : ICommand
    {
        private readonly List<Tuple<ICommand, ICommand>> _actions;

        public SagaCommand(List<Tuple<ICommand, ICommand>> actions)
        {
            _actions = actions;
        }

        public void Execute()
        {
            int executedCount = 0;
            try
            {
                executedCount = _actions.Aggregate(0, (count, action) =>
                {
                    action.Item1.Execute();
                    return count + 1;
                });

            }
            catch
            {
                var undoCommands = _actions
                .Take(executedCount)
                .Select(a => a.Item2)
                .Reverse()
                .ToList();

                new MacroCommand(undoCommands).Execute();
                throw;
            }

        }
    }
}
