using Hwdtech;


namespace SpaceBattle;

public class CreateSaga: IStrategy  
{
    public object ExecuteStrategy(params object[] args)
    {
        List<string> cmdNames = new List<string>();
        for (int i=0; i<args.Length-3; i++){

            cmdNames.Add((string) args[i]);

        }

        string pivotName = (string)args[args.Length - 3];
        IUObject obj = (IUObject) args[args.Length-2];
        int maxRetries = (int)args[args.Length - 1];

        List<Tuple<ICommand, ICommand>> beforePivot = new List<Tuple<ICommand, ICommand>>();
        List<Tuple<ICommand, ICommand>> afterPivot = new List<Tuple<ICommand, ICommand>>();
        ICommand? pivotCommand = null;

        bool pivotFound = false;

        foreach (string name in cmdNames){
            var cmd = IoC.Resolve<ICommand>(name, obj, 3);

            if (name == pivotName){
                pivotCommand = cmd;
                pivotFound = true;
                continue;
            }

            if (!pivotFound) {
                var undo = IoC.Resolve<ICommand>("Undo." + name, obj);
                beforePivot.Add(Tuple.Create(cmd, undo));
            }
            else {
                var undo = IoC.Resolve<ICommand>("Undo." + name, obj);
                beforePivot.Add(Tuple.Create(cmd, undo));
            }
        }

        if (pivotCommand is null) {
            throw new InvalidOperationException("Pivot command not found.");
        }
        return new SagaCommand(beforePivot, pivotCommand, afterPivot, maxRetries);
    }
}
    