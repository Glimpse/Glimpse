using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class RuntimePolicyContextShould
    {
        [Fact]
        public void Constuct()
        {
            var metadataMock = new Mock<IRequestMetadata>();
            var loggerMock = new Mock<ILogger>();
            var requestContext = new DummyObjectContext();

            var context = new RuntimePolicyContext(metadataMock.Object, loggerMock.Object, requestContext);

            Assert.Equal(metadataMock.Object, context.RequestMetadata);
            Assert.Equal(loggerMock.Object, context.Logger);
            Assert.Equal(requestContext, context.GetRequestContext<DummyObjectContext>());
        }

        [Fact]
        public void ThrowExceptionWithNullRequestMetadata()
        {
            var loggerMock = new Mock<ILogger>();
            var requestContext = new DummyObjectContext();

            Assert.Throws<ArgumentNullException>(() => new RuntimePolicyContext(null, loggerMock.Object, requestContext));
        }

        [Fact]
        public void ThrowExceptionWithNullLogger()
        {
            var metadataMock = new Mock<IRequestMetadata>();
            var requestContext = new DummyObjectContext();

            Assert.Throws<ArgumentNullException>(() => new RuntimePolicyContext(metadataMock.Object, null, requestContext));
        }

        [Fact]
        public void ThrowExceptionWithNullRequestContext()
        {
            var metadataMock = new Mock<IRequestMetadata>();
            var loggerMock = new Mock<ILogger>();

            Assert.Throws<ArgumentNullException>(() => new RuntimePolicyContext(metadataMock.Object, loggerMock.Object, null));
        }

        [Fact]
        public void ReturnNullContextWithInvalidType()
        {
            var metadataMock = new Mock<IRequestMetadata>();
            var loggerMock = new Mock<ILogger>();

            var context = new RuntimePolicyContext(metadataMock.Object, loggerMock.Object, "not a good context");

            Assert.Null(context.GetRequestContext<DummyObjectContext>());
        }
    }
}