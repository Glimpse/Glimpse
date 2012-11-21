using System;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
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

        [Fact]
        public void ThrowCastErrorWithTypeMisMatch()
        {
            var converterMock = new Mock<SerializationConverter<DummyObjectContext>>();

            Assert.Throws<InvalidCastException>(()=>converterMock.Object.Convert("break me"));
        }

        [Fact]
        public void CallAbstractConvert()
        {
            var converterMock = new Mock<SerializationConverter<DummyObjectContext>>();

            var objContext = new DummyObjectContext();
            converterMock.Object.Convert(objContext);

            converterMock.Verify(c=>c.Convert(objContext), Times.Once());
        }
    }
}