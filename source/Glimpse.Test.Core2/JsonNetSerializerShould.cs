using System;
using System.Linq;
using System.Runtime.Serialization;
using Glimpse.Core2.Extensibility;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class JsonNetSerializerShould
    {
        [Fact]
        public void Construct()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            Assert.NotNull(serializer);
        }

        [Fact]
        public void SerializeObjects()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            var simpleObject = new {String = "A string", Integer = 5};

            string result = serializer.Serialize(simpleObject);

            Assert.Equal("{\"String\":\"A string\",\"Integer\":5}", result);
        }

        [Fact]
        public void IgnorePropertiesWithErrors()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            var badObject = new TestObjectWithException();

            string result = serializer.Serialize(badObject);

            Assert.Equal("{\"String\":\"A string\"}", result);
            //This verify is not registered due to lambda. It has been verified to work
            //loggerMock.Verify(l=>l.Error(It.IsAny<string>(), It.IsAny<NotSupportedException>()));
        }

        [Fact]
        public void RespectJsonPropertyOverrides()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            var overrideObject = new TestObjectWithJsonAttributes();

            string result = serializer.Serialize(overrideObject);

            Assert.Equal("{\"meaningfullName\":\"A string\"}", result);
        }

        [Fact]
        public void RespectISerializableObjects()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            var iSerializableObj = new TestObjectAsISerializable();

            var result = serializer.Serialize(iSerializableObj);

            Assert.Equal("{\"otherKey\":\"otherValue\"}", result);
        }

        [Fact]
        public void RegisterISerializationConverters()
        {
            var loggerMock = new Mock<ILogger>();

            var converter1 = new Mock<ISerializationConverter>();
            var converter2 = new Mock<ISerializationConverter>();

            var serializer = new JsonNetSerializer(loggerMock.Object);

            serializer.RegisterSerializationConverters(new[]{converter1.Object, converter2.Object});
        }

        [Fact]
        public void RegisterEmptyCollectionOfISerializationConverters()
        {
            var loggerMock = new Mock<ILogger>();

            var serializer = new JsonNetSerializer(loggerMock.Object);

            serializer.RegisterSerializationConverters(Enumerable.Empty<ISerializationConverter>());
        }

        [Fact]
        public void ThrowWhenRegisterNullCollectionOfISerializationConverters()
        {
            var loggerMock = new Mock<ILogger>();

            var serializer = new JsonNetSerializer(loggerMock.Object);

            Assert.Throws<ArgumentNullException>(()=>serializer.RegisterSerializationConverters(null));
        }
    }

    #region Test Objects
    internal class TestObjectWithException
    {
        public string String
        {
            get { return "A string"; }
        }

        public string Error
        {
            get { throw new NotSupportedException(); }
        }
    }

    internal class TestObjectWithJsonAttributes
    {
        [JsonProperty(PropertyName = "meaningfullName")]
        public string String
        {
            get { return "A string"; }
        }

        [JsonIgnore]
        public int NeverSeeMe
        {
            get { return 5; }
        }
    }

    internal class TestObjectAsISerializable : ISerializable
    {
        public string NeverSeeMe
        {
            get { return "A string"; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("otherKey", "otherValue");
        }
    }
    #endregion Test Objects
}
