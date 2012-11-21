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
            var result = new StatusCodeResourceResult(101);
            Assert.Equal(101, result.StatusCode);
        }

        [Fact]
        public void Execute()
        {
            var frameworkProviderMock = new Mock<IFrameworkProvider>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(frameworkProviderMock.Object);

            var result = new StatusCodeResourceResult(101);

            result.Execute(contextMock.Object);

            frameworkProviderMock.Verify(fp=>fp.SetHttpResponseStatusCode(101), Times.Once());
        }
    }
}