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
            var undoCommands = new List<ICommand>();
            try
            {
                undoCommands = _actions.Aggregate(new List<ICommand>(), (undoList, action) =>
                {
                    action.Item1.Execute();
                    undoList.Insert(0, action.Item2);
                    return undoList;
                });

            }
            catch
            {
                new MacroCommand(undoCommands).Execute();
                throw;
            }

        }
    }
}
