using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class ExceptionResourceResultShould
    {
        [Fact]
        public void Construct()
        {
            var exception = new DummyException("This is a dummy");

            var resourceResult = new ExceptionResourceResult(exception);

            Assert.Equal(exception, resourceResult.Exception);
        }

        [Fact]
        public void Execute()
        {
            var providerMock = new Mock<IFrameworkProvider>();
            var contextMock = new Mock<IResourceResultContext>();
            contextMock.Setup(c => c.FrameworkProvider).Returns(providerMock.Object);

            var exception = new DummyException("This is a dummy");

            var resourceResult = new ExceptionResourceResult(exception);

            resourceResult.Execute(contextMock.Object);

            providerMock.Verify(p=>p.WriteHttpResponse(exception.ToString()));
            providerMock.Verify(p=>p.SetHttpResponseStatusCode(500));
        }
    }
}