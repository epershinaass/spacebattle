using Hwdtech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpaceBattle
{
    public class CreateSaga : IStrategy
    {
        public object ExecuteStrategy(params object[] args)
        {
            var CmdNames = args[0] as List<List<string>> ?? throw new ArgumentNullException("CmdNames");
            var obj = args[1] as IUObject ?? throw new ArgumentNullException("obj");
            var maxRetries = (int)args[2];

            var retrySagas = CmdNames.Select(group =>
            {
                var cmdPairs = group.Select(name =>
                {
                    var doCmd = IoC.Resolve<ICommand>(name, obj);
                    var undoCmd = IoC.Resolve<ICommand>($"Undo.{name}", obj);
                    return Tuple.Create(doCmd, undoCmd);
                }).ToList();

                var saga = new SagaCommand(cmdPairs);
                return (ICommand)new RetryCommand(saga, maxRetries);
            }).ToList();

            return new MacroCommand(retrySagas);
        }
    }
}
