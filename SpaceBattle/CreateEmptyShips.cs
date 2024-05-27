using Hwdtech;
namespace SpaceBattle;

public class CreateEmptyShips : IStrategy
{
    public object ExecuteStrategy(params object[] args)
    {
        throw new NotImplementedException();
    }

    public object RunStrategy(params object[] args)
    {
        Dictionary<string, IUObject> gameObjects = IoC.Resolve<Dictionary<string, IUObject>>("General.Objects");
        Dictionary<string, object> gameParams = IoC.Resolve<Dictionary<string, object>>("Game.InitProperties");
        int numOfPlayers = (int) gameParams["numberOfPlayers"];
        int shipsPerPlayer = (int) gameParams["shipsPerPlayer"];
        for (int i = 0; i < numOfPlayers; i++)
        {
            string playerId = IoC.Resolve<string>("General.AddNewPlayer");
            for (int j = 0; j < shipsPerPlayer; j++)
            {
                IUObject newObj = IoC.Resolve<IUObject>("General.Objects.Empty");
                newObj.SetProperty("player", playerId);
                gameObjects.Add(IoC.Resolve<string>("General.Objects.EmptyId"), newObj);
            }
        }
        return new object();
    }
}
