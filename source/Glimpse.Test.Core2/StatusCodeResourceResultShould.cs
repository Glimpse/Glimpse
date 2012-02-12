using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
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