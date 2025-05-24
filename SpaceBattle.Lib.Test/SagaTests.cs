using Xunit;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Generic;

namespace SpaceBattle.Lib.Test
{
    public class SagaTests
    {
        public SagaTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void SagaSucsessTest()
        {
            var obj = new TestObject(new Dictionary<string, object>());
            obj.SetProperty("Position", new Vector(12, 5));
            obj.SetProperty("Velocity", new Vector(-7, 3));
            obj.SetProperty("fuelLevel", 100f);
            obj.SetProperty("fuelConsumption", 1f);

            var mockMovable = new Mock<IMovable>();
            mockMovable.SetupGet(m => m.Position).Returns((Vector)obj.GetProperty("Position"));
            mockMovable.SetupGet(m => m.Velocity).Returns((Vector)obj.GetProperty("Velocity"));

            var mockFuel = new Mock<IFuelChangable>();
            mockFuel.SetupGet(f => f.fuelLevel).Returns((float)obj.GetProperty("fuelLevel"));
            mockFuel.SetupGet(f => f.fuelConsumption).Returns((float)obj.GetProperty("fuelConsumption"));
            mockFuel.SetupSet(f => f.fuelLevel = 99f).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MoveCommand", (object[] args) => new MoveCommand(mockMovable.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "WasteFuelCommand", (object[] args) => new WasteFuelCommand(mockFuel.Object)).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.MoveCommand", (object[] args) => new ActionCommand(() =>
            {
                mockMovable.Object.Position -= mockMovable.Object.Velocity;
            })).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.WasteFuelCommand", (object[] args) => new ActionCommand(() =>
       {
           mockFuel.Object.fuelLevel += mockFuel.Object.fuelConsumption;
       })).Execute();

            var commandsList = new List<List<string>> { new List<string> { "MoveCommand", "WasteFuelCommand" } };
            var saga = (ICommand)new CreateSaga().ExecuteStrategy(commandsList, obj, 3);
            saga.Execute();

            mockFuel.VerifyGet(f => f.fuelLevel, Times.Once);
            mockFuel.VerifyGet(f => f.fuelConsumption, Times.Once);
            mockFuel.VerifySet(f => f.fuelLevel = 99f, Times.Once);

            mockMovable.VerifyGet(m => m.Position, Times.Once);
            mockMovable.VerifyGet(m => m.Velocity, Times.Once);
        }

        [Fact]
        public void TwoSagasSecondFailsTest()
        {
            var obj = new TestObject(new Dictionary<string, object>());
            obj.SetProperty("Position", new Vector(12, 5));
            obj.SetProperty("Velocity", new Vector(-7, 3));
            obj.SetProperty("fuelLevel", 100f);
            obj.SetProperty("fuelConsumption", 1f);

            var mockMovable1 = new Mock<IMovable>();
            mockMovable1.SetupGet(m => m.Position).Returns((Vector)obj.GetProperty("Position"));
            mockMovable1.SetupGet(m => m.Velocity).Returns((Vector)obj.GetProperty("Velocity"));

            var mockFuel1 = new Mock<IFuelChangable>();
            mockFuel1.SetupGet(f => f.fuelLevel).Returns((float)obj.GetProperty("fuelLevel"));
            mockFuel1.SetupGet(f => f.fuelConsumption).Returns((float)obj.GetProperty("fuelConsumption"));
            mockFuel1.SetupSet(f => f.fuelLevel = 99f).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "FirstMoveCommand", (object[] args) => new MoveCommand(mockMovable1.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "FirstWasteFuelCommand", (object[] args) => new WasteFuelCommand(mockFuel1.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.FirstMoveCommand", (object[] args) => new ActionCommand(() =>
            {
                mockMovable1.Object.Position -= mockMovable1.Object.Velocity;
            })).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.FirstWasteFuelCommand", (object[] args) => new ActionCommand(() =>
            {
                mockFuel1.Object.fuelLevel += mockFuel1.Object.fuelConsumption;
            })).Execute();

            // 2) Вторая сага: Move падает на первой попытке, Waste только во второй итерации
            var mockMovable2 = new Mock<IMovable>();
            int move2Calls = 0;
            mockMovable2.SetupGet(m => m.Position).Returns(() =>
            {
                if (move2Calls++ == 0)
                    throw new Exception("fail SecondMoveCommand first attempt");
                return (Vector)obj.GetProperty("Position");
            });
            mockMovable2.SetupGet(m => m.Velocity).Returns((Vector)obj.GetProperty("Velocity"));

            var mockFuel2 = new Mock<IFuelChangable>();
            mockFuel2.SetupGet(f => f.fuelLevel).Returns((float)obj.GetProperty("fuelLevel"));
            mockFuel2.SetupGet(f => f.fuelConsumption).Returns((float)obj.GetProperty("fuelConsumption"));
            mockFuel2.SetupSet(f => f.fuelLevel = It.IsAny<float>()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SecondMoveCommand", (object[] args) => new MoveCommand(mockMovable2.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SecondWasteFuelCommand", (object[] args) => new WasteFuelCommand(mockFuel2.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.SecondMoveCommand", (object[] args) => new ActionCommand(() =>
            {
                mockMovable2.Object.Position -= mockMovable2.Object.Velocity;
            })).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.SecondWasteFuelCommand", (object[] args) => new ActionCommand(() =>
            {
                mockFuel2.Object.fuelLevel += mockFuel2.Object.fuelConsumption;
            })).Execute();

            var commandsList = new List<List<string>>
            {
                new List<string> { "FirstMoveCommand", "FirstWasteFuelCommand" },
                new List<string> { "SecondMoveCommand", "SecondWasteFuelCommand" }
            };

            var saga = (ICommand)new CreateSaga().ExecuteStrategy(commandsList, obj, 1);

            saga.Execute();

            // — первая сага прошла без отката, команды отработали ровно по одному разу:
            mockMovable1.VerifyGet(m => m.Position, Times.Once);
            mockMovable1.VerifyGet(m => m.Velocity, Times.Once);
            mockFuel1.VerifyGet(f => f.fuelLevel, Times.Once);
            mockFuel1.VerifyGet(f => f.fuelConsumption, Times.Once);
            mockFuel1.VerifySet(f => f.fuelLevel = 99f, Times.Once);

            // — вторая сага: Move попытался дважды (первая упала, вторая успешна), Velocity один раз, Waste только во второй попытке
            mockMovable2.VerifyGet(m => m.Position, Times.Exactly(2));
            mockMovable2.VerifyGet(m => m.Velocity, Times.Once);
            mockFuel2.VerifyGet(f => f.fuelLevel, Times.Once);
            mockFuel2.VerifyGet(f => f.fuelConsumption, Times.Once);
            mockFuel2.VerifySet(f => f.fuelLevel = It.IsAny<float>(), Times.AtLeastOnce);
        }

        [Fact]
        public void SagaCommandWithRetry()
        {
            var obj = new TestObject(new Dictionary<string, object>());
            obj.SetProperty("Position", new Vector(12, 5));
            obj.SetProperty("Velocity", new Vector(-7, 3));
            obj.SetProperty("fuelLevel", 100f);
            obj.SetProperty("fuelConsumption", 1f);

            var mockMovable = new Mock<IMovable>();
            var callCount = 0;

            mockMovable.SetupGet(m => m.Position).Returns(() =>
            {
                if (callCount == 0)
                {
                    callCount++;
                    throw new Exception("Initial failure");
                }
                return (Vector)obj.GetProperty("Position");
            });

            mockMovable.SetupGet(m => m.Velocity).Returns((Vector)obj.GetProperty("Velocity"));

            var mockFuel = new Mock<IFuelChangable>();

            mockFuel.SetupGet(f => f.fuelLevel).Returns((float)obj.GetProperty("fuelLevel"));
            mockFuel.SetupGet(f => f.fuelConsumption).Returns((float)obj.GetProperty("fuelConsumption"));
            mockFuel.SetupSet(f => f.fuelLevel = It.IsAny<float>()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MoveCommand", (object[] args) =>
                new MoveCommand(mockMovable.Object)).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "WasteFuelCommand", (object[] args) =>
                new WasteFuelCommand(mockFuel.Object)).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.MoveCommand", (object[] args) =>
                new ActionCommand(() =>
                {
                    obj.SetProperty("Position", ((Vector)obj.GetProperty("Position")) - ((Vector)obj.GetProperty("Velocity")));
                })).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Undo.WasteFuelCommand", (object[] args) =>
                new ActionCommand(() =>
                {
                    mockFuel.Object.fuelLevel += mockFuel.Object.fuelConsumption;
                })).Execute();

            var commandsList = new List<List<string>> { new List<string> { "MoveCommand", "WasteFuelCommand" } };
            var sagaWithRetry = (ICommand)new CreateSaga().ExecuteStrategy(commandsList, obj, 1); // Retry 1 раз

            sagaWithRetry.Execute();

            // Ожидаем 2 вызова Position: первый падает, второй - нет
            mockMovable.VerifyGet(m => m.Position, Times.Exactly(2));

            // после первого обвала WasteFuelCommand не исполнялся, но во второй итерации должен
            mockFuel.VerifyGet(f => f.fuelLevel, Times.AtLeastOnce);
            mockFuel.VerifyGet(f => f.fuelConsumption, Times.AtLeastOnce);
            mockFuel.VerifySet(f => f.fuelLevel = It.IsAny<float>(), Times.AtLeastOnce);
        }
    }
}
