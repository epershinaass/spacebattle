using Hwdtech;

namespace SpaceBattle;

public class CreateSteppSaga : IStrategy
{
    public object ExecuteStrategy(params object[] args)
    {
        var commandNames = args.Take(args.Length - 1).Cast<string>().ToList();
        var target = (IUObject)args.Last();

        var steps = commandNames.Select(name =>
        {
            var doCommand = IoC.Resolve<ICommand>(name, target);
            var undoCommand = IoC.Resolve<ICommand>("Undo." + name, target);
            return (ISagaStep)new SagaStep(doCommand, undoCommand); 
        }).ToList();

        return new SagaCommand(steps);
    }
}