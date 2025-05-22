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
            var undoStack = new Stack<ICommand>();

            foreach (var (doCmd, undoCmd) in _actions)
            {
                try
                {
                    doCmd.Execute();
                    undoStack.Push(undoCmd);
                }
                catch
                {
                    foreach (var undo in undoStack)
                    {
                        try
                        {
                            undo.Execute();
                        }
                        catch
                        {
                            
                        }
                    }

                    throw;
                }
            }
        }
    }
}
