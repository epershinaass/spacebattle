namespace SpaceBattle;
using Hwdtech;

public class CheckCollision : ICommand
{
    IUObject obj1
    {
        get;
    }
    IUObject obj2
    {
        get;
    }

    public CheckCollision(IUObject obj1, IUObject obj2)
    {
        this.obj1 = obj1;
        this.obj2 = obj2;
    }

    public void Execute()
    {
        var list = IoC.Resolve<List<Vector>>("SpaceBattle.GetDifference", obj1, obj2);
        bool collision = IoC.Resolve<bool>("SpaceBattle.GetDecisionTree", list);
        if (collision) throw new Exception();
    }
}