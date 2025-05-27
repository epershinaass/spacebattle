using Moq;
using Xunit;
using System;

namespace SpaceBattle.Lib.Test
{
    public class RetryCommandTests
    {
        [Fact]
        public void Test_Success_After_Retries()
        {
            var mockCommand = new Mock<ICommand>();

            int callCount = 0;

            mockCommand.Setup(m => m.Execute()).Callback(() =>
            {
                callCount++;
                if (callCount < 3)
                {
                    throw new Exception();
                }
            });

            var retryCommand = new RetryCommand(mockCommand.Object, maxRetries: 5);

            retryCommand.Execute();

            Assert.Equal(3, callCount);
        }

        [Fact]
        public void Test_MaxRetriesExceeded_Throws()
        {
            
            var mockCommand = new Mock<ICommand>();

            mockCommand.Setup(m => m.Execute()).Throws<InvalidOperationException>();

            var retryCommand = new RetryCommand(mockCommand.Object, maxRetries: 2);

            Assert.Throws<InvalidOperationException>(() => retryCommand.Execute());

            mockCommand.Verify(m => m.Execute(), Times.Exactly(3));
        }
    }
}
