namespace SpaceBattle;

public class SagaCommand : ICommand
{
    List<Tuple<ICommand, ICommand>> cmds;
    int pivotIndex;
    int maxRetries;
    public SagaCommand(List<Tuple<ICommand, ICommand>> _cmds, int _pivotIndex = -1, int _maxRetries = 0)
    {
        cmds = _cmds;
        pivotIndex = _pivotIndex;
        maxRetries = _maxRetries;
    }      
    public void Execute()
    {
        int i = 0;
        try {    
        for (; i < cmds.Count(); i++) {
            int attempt = 0;
            while (true)
            {
                try{
                    cmds[i].Item1.Execute();
                    break;} 
                catch {
                attempt++;
                if (attempt > maxRetries)
                throw;}
            }
            }
        }catch{
            i -= 1;
            for (; i >= 0; i--){
                if (pivotIndex == -1 || i <= pivotIndex){
                    cmds[i].Item2.Execute();}
        }
    }
}
}
