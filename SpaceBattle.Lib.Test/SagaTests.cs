namespace SpaceBattle.Lib.Test;

using Moq;
using Hwdtech;
using Hwdtech.Ioc;
public class SagaTests
{
    public SagaTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }  
    
    [Fact]
    public void executeSagaSuccessTest()
    {
        var obj = new TestObject(new Dictionary<string, object>());
        obj.SetProperty("Position", new Vector(12, 5));
        obj.SetProperty("Velocity", new Vector(-7, 3));
        obj.SetProperty("fuelLevel", (float) 100);
        obj.SetProperty("fuelConsumption", (float) 1);

        var mockIMAdapter = new Mock<IMovable>();
        mockIMAdapter.SetupGet(x => x.Position).Returns((Vector) obj.GetProperty("Position"));
        mockIMAdapter.SetupGet(x => x.Velocity).Returns((Vector) obj.GetProperty("Velocity"));

        mockIMAdapter.SetupSet(x => x.Position = new Vector(5, 8)).Verifiable();
 
        var mockIWAdapter = new Mock<IFuelChangable>();
        mockIWAdapter.SetupGet(x => x.fuelLevel).Returns((float) obj.GetProperty("fuelLevel"));
        mockIWAdapter.SetupGet(x => x.fuelConsumption).Returns((float) obj.GetProperty("fuelConsumption"));

        mockIWAdapter.SetupSet(x => x.fuelLevel = 99).Verifiable();
        

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MoveCommand", (object[] args) => new RetryCommand(new MoveCommand(mockIMAdapter.Object), (int)args[1])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "FuelWasteCommand", (object[] args) =>  new RetryCommand(new WasteFuelCommand(mockIWAdapter.Object), (int)args[1])).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.MoveCommand", (object[] args) => new MoveCommand(mockIMAdapter.Object)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.FuelWasteCommand", (object[] args) => new WasteFuelCommand(mockIWAdapter.Object)).Execute();

        SagaCommand sc = (SagaCommand) new CreateSaga().ExecuteStrategy("MoveCommand", "FuelWasteCommand", "", obj);
        sc.Execute();

        mockIMAdapter.VerifyGet(x => x.Position, Times.Once);
        mockIMAdapter.VerifyGet(x => x.Velocity, Times.Once);
        mockIWAdapter.VerifyGet(x => x.fuelLevel, Times.Once);
        mockIWAdapter.VerifyGet(x => x.fuelConsumption, Times.Once);

        mockIMAdapter.Verify();
        mockIWAdapter.Verify();
    }

    [Fact]
    public void executeSagaPivotTest()
    {
        var obj = new TestObject(new Dictionary<string, object>());
        obj.SetProperty("Position", new Vector(12, 5));
        obj.SetProperty("Velocity", new Vector(-7, 3));
        obj.SetProperty("fuelLevel", (float)100);
        obj.SetProperty("fuelConsumption", (float)1);

        var mockIMAdapter = new Mock<IMovable>();
        mockIMAdapter.SetupGet(x => x.Position).Returns((Vector)obj.GetProperty("Position"));
        mockIMAdapter.SetupGet(x => x.Velocity).Returns((Vector)obj.GetProperty("Velocity"));

        mockIMAdapter.SetupSet(x => x.Position = new Vector(5, 8)).Verifiable();
    
        var mockIWAdapter = new Mock<IFuelChangable>();
        mockIWAdapter.SetupGet(x => x.fuelLevel).Returns((float)obj.GetProperty("fuelLevel"));
        mockIWAdapter.SetupGet(x => x.fuelConsumption).Returns((float)obj.GetProperty("fuelConsumption"));

        mockIWAdapter.SetupSet(x => x.fuelLevel = 99).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MoveCommand", (object[] args) => new MoveCommand(mockIMAdapter.Object)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "FuelWasteCommand", (object[] args) => new WasteFuelCommand(mockIWAdapter.Object)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.MoveCommand", (object[] args) => new MoveCommand(mockIMAdapter.Object)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.FuelWasteCommand", (object[] args) => new WasteFuelCommand(mockIWAdapter.Object)).Execute();

        string pivotName = "FuelWasteCommand";
        SagaCommand sc = (SagaCommand)new CreateSaga().ExecuteStrategy("MoveCommand", pivotName, "MoveCommand", obj);
        sc.Execute();

        mockIMAdapter.VerifyGet(x => x.Position, Times.Once);
        mockIMAdapter.VerifyGet(x => x.Velocity, Times.Once);
        mockIWAdapter.VerifyGet(x => x.fuelLevel, Times.Once);
        mockIWAdapter.VerifyGet(x => x.fuelConsumption, Times.Once);

        mockIMAdapter.Verify();
        mockIWAdapter.Verify();

        mockIWAdapter.VerifySet(x => x.fuelLevel = 99, Times.Once); 
    }
}
