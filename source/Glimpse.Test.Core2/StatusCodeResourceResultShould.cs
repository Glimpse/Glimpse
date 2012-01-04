using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
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

            var result = new StatusCodeResourceResult(101);

            result.Execute(frameworkProviderMock.Object);

            frameworkProviderMock.Verify(fp=>fp.SetHttpResponseStatusCode(101), Times.Once());
        }
    }
}