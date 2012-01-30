using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class SerializationConverterShould
    {
        [Fact]
        public void ReturnSupportedType()
        {
            var converterMock = new Mock<SerializationConverter<DummyObjectContext>>();

            Assert.Equal(1, converterMock.Object.SupportedTypes.Count());
            Assert.Equal(typeof(DummyObjectContext), converterMock.Object.SupportedTypes.First());
        }
    }
}