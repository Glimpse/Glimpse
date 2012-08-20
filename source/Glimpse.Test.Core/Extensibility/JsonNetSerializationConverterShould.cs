using System;
using System.IO;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class JsonNetSerializationConverterShould
    {
        [Fact]
        public void Construct()
        {
            var converterMock = new Mock<ISerializationConverter>();

            Assert.NotNull(new JsonNetSerializationConverterAdapter(converterMock.Object));
        }

        [Fact]
        public void WriteJson()
        {
            var converterMock = new Mock<ISerializationConverter>();

            var adapter = new JsonNetSerializationConverterAdapter(converterMock.Object);

            var jsonTextWriter = new JsonTextWriter(new StringWriter(new StringBuilder()));
            var obj = new {It = "Any"};
            var jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings());
            adapter.WriteJson(jsonTextWriter,  obj, jsonSerializer);

            converterMock.Verify(c=>c.Convert(obj), Times.Once());
        }

        [Fact]
        public void ReadJson()
        {
            var converterMock = new Mock<ISerializationConverter>();

            var adapter = new JsonNetSerializationConverterAdapter(converterMock.Object);

            Assert.Throws<NotSupportedException>(()=>adapter.ReadJson(null, null, null, null));
        }

        [Fact]
        public void CanConvertWithMatch()
        {
            var converterMock = new Mock<ISerializationConverter>();
            converterMock.Setup(c => c.SupportedTypes).Returns(new[] { typeof(DummyObjectContext), typeof(string) });

            var adapter = new JsonNetSerializationConverterAdapter(converterMock.Object);

            Assert.True(adapter.CanConvert(typeof(DummyObjectContext)));
        }

        [Fact]
        public void CanConvertWithoutMatch()
        {
            var converterMock = new Mock<ISerializationConverter>();
            converterMock.Setup(c => c.SupportedTypes).Returns(new[] { typeof(DummyObjectContext), typeof(string) });

            var adapter = new JsonNetSerializationConverterAdapter(converterMock.Object);

            Assert.False(adapter.CanConvert(typeof(int)));
        }
    }
}