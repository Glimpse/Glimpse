using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using Glimpse.Core.Extensibility;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Glimpse.Test.Core
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

            Assert.Equal("{\"string\":\"A string\",\"integer\":5}", result);
        }

        [Fact]
        public void IgnorePropertiesWithErrors()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            var badObject = new TestObjectWithException();

            string result = serializer.Serialize(badObject);

            Assert.Equal("{\"string\":\"A string\"}", result);
            loggerMock.Verify(l => l.Error(It.IsAny<string>(),
                                           It.Is<JsonException>(ex => ex.InnerException is NotSupportedException)));
        }

        [Fact]
        public void IgnoreListReferenceLoop()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            var loop = new TestObjectWithListReferenceLoop();
            loop.Add(loop);

            string result = serializer.Serialize(loop);

            Assert.Equal("[]", result);
        }

        [Fact]
        public void IgnoreEnumerableReferenceLoop()
        {
            var loggerMock = new Mock<ILogger>();

            ISerializer serializer = new JsonNetSerializer(loggerMock.Object);

            var loop = new TestObjectWithEnumerableReferenceLoop();

            string result = serializer.Serialize(loop);

            Assert.Equal("[]", result);
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

    public class TestObjectWithListReferenceLoop : ArrayList
    {
    }

    public class TestObjectWithEnumerableReferenceLoop : IEnumerable
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this;
        }
    }
    #endregion Test Objects
}
