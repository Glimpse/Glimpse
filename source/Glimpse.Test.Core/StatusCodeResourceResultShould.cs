using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core
{
    public class StatusCodeResourceResultShould
    {
        [Fact]
        public void ConstructWithStatusCode()
        {
            var expectedMessage = "any message";
            var result = new StatusCodeResourceResult(101, expectedMessage);
            Assert.Equal(101, result.StatusCode);
            Assert.Equal(expectedMessage, result.Message);
        }

        [Fact]
        public void Execute()
        {
            var frameworkProviderMock = new Mock<IRequestResponseAdapter>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.RequestResponseAdapter).Returns(frameworkProviderMock.Object);

            var result = new StatusCodeResourceResult(101, "Message");

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp=>fp.SetHttpResponseStatusCode(101), Times.Once());
        }
    }
}