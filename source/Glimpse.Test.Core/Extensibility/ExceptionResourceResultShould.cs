using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class ExceptionResourceResultShould
    {
        [Fact]
        public void Construct()
        {
            var exception = new DummyException("This is a dummy");

            var resourceResult = new ExceptionResourceResult(exception);

            Assert.Equal(exception.ToString(), resourceResult.Message);
        }

        [Fact]
        public void Execute()
        {
            var providerMock = new Mock<IRequestResponseAdapter>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.RequestResponseAdapter).Returns(providerMock.Object);

            var exception = new DummyException("This is a dummy");

            var resourceResult = new ExceptionResourceResult(exception);

            resourceResult.Execute(contextMock.Object);

            providerMock.Verify(p=>p.WriteHttpResponse(exception.ToString()));
            providerMock.Verify(p=>p.SetHttpResponseStatusCode(500));
        }
    }
}