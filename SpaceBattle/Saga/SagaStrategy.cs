using Hwdtech;


namespace SpaceBattle;

public class CreateSaga: IStrategy  
{
    public object ExecuteStrategy(params object[] args)
    {
        List<string> cmdNames = new List<string>();
        for (int i=0; i<args.Length-2; i++){

            cmdNames.Add((string) args[i]);

        }

        string pivotName = (string)args[args.Length - 2];
        IUObject obj = (IUObject) args[args.Length-1];

        List<Tuple<ICommand, ICommand>> cmds = new List<Tuple<ICommand, ICommand>>();
        int pivotIndex = -1;

        for (int i = 0; i<cmdNames.Count; i++){
            string name = cmdNames[i];
            var cmd = IoC.Resolve<ICommand>(name, obj);
            var undo = IoC.Resolve<ICommand>("Undo." + name, obj);
            cmds.Add(Tuple.Create(cmd, undo));
            
            if (name == pivotName)
            {
                pivotIndex = i;
            }
            
        }

        return new SagaCommand(cmds, pivotIndex);
    }
    
}
