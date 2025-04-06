using Hwdtech;


namespace SpaceBattle;

public class CreateSaga: IStrategy  
{
    public object ExecuteStrategy(params object[] args)
    {
        List<string> cmdNames = new List<string>();
        for (int i=0; i<args.Length-1; i++){
            cmdNames.Add((string) args[i]);
        }
        IUObject obj = (IUObject) args[args.Length-1];

        List<Tuple<ICommand, ICommand>> cmds = new List<Tuple<ICommand, ICommand>>();

        cmdNames.ForEach(name => 
            {
                cmds.Add(new Tuple<ICommand, ICommand>(IoC.Resolve<ICommand>(name, obj), IoC.Resolve<ICommand>("Undo." + name, obj)));
            }
        );

        return new SagaCommand(cmds);
    }
    
}