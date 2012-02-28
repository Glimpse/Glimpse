using System.Runtime.Serialization;
using Glimpse.Core2;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseExceptionShould
    {
        [Fact]
        public void Construct()
        {
            Assert.NotNull(new GlimpseException());
        }

        [Fact]
        public void ConstructWithMessage()
        {
            var message = "Testing";
            var exception = new GlimpseException(message);

            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void ConstructWithMessageAndInnerException()
        {
            var message = "Testing";
            var innerException = new DummyException();
            var exception = new GlimpseException(message, innerException);

            Assert.Equal(message, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact(Skip = "This method is hard to test and is framework code anyways.")]
        public void ConstructWithSerializationInfoAndStreamingContext()
        {
            var formatConverterMock = new Mock<IFormatterConverter>();

            var info = new SerializationInfo(typeof (GlimpseExceptionShould), formatConverterMock.Object);

            var context = new StreamingContext();

            var exception = new GlimpseException(info, context);

            Assert.NotNull(exception);
        }
    }
}