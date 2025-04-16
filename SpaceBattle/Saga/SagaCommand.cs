namespace SpaceBattle;

public class SagaCommand : ICommand
{
    List<Tuple<ICommand, ICommand>> cmds;
    int pivotIndex;
    public SagaCommand(List<Tuple<ICommand, ICommand>> _cmds, int _pivotIndex = -1)
    {
        cmds = _cmds;
        pivotIndex = _pivotIndex;
    }      
    public void Execute()
    {
        int i = 0;
        try
        {
            for (; i < cmds.Count(); i++)
            {
                cmds[i].Item1.Execute();
            }
        } catch {
            i -= 1;
            for (; i >= 0; i--)
            {
                if (pivotIndex == -1 || i<= pivotIndex)
                {
                    cmds[i].Item2.Execute();
                }
            }
        }
    }
}
