using Hwdtech;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceBattle
{
    public class CreateSaga : IStrategy
    {
        public object ExecuteStrategy(params object[] args)
        {
            var cmdNames = args.Take(args.Length - 3).Cast<string>().ToList();
            var pivotName = (string)args[^3];
            var target = (IUObject)args[^2];
            var maxRetries = (int)args[^1];

            var sagas = new List<ICommand>();
            var currentActions = new List<Tuple<ICommand, ICommand>>();

            foreach (var name in cmdNames)
            {
                if (name == pivotName)
                {
                    if (currentActions.Any())
                        sagas.Add(new SagaCommand(currentActions));

                    sagas.Add(IoC.Resolve<ICommand>(name, target, 3));
                    currentActions = new List<Tuple<ICommand, ICommand>>();
                }
                else
                {
                    var cmd = IoC.Resolve<ICommand>(name, target, 3);
                    var undo = IoC.Resolve<ICommand>($"Undo.{name}", target);
                    currentActions.Add(Tuple.Create(cmd, undo));
                }
            }

            if (currentActions.Any())
                sagas.Add(new SagaCommand(currentActions));

            return new RetryCommand(new MacroCommand(sagas), maxRetries);
        }
    }
}
