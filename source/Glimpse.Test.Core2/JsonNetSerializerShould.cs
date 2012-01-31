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
            ISerializer serializer = new JsonNetSerializer();

            Assert.NotNull(serializer);
        }

        [Fact]
        public void SerializeObjects()
        {
            ISerializer serializer = new JsonNetSerializer();

            var simpleObject = new {String = "A string", Integer = 5};

            string result = serializer.Serialize(simpleObject);

            Assert.Equal("{\"String\":\"A string\",\"Integer\":5}", result);
        }

        [Fact]
        public void IgnorePropertiesWithErrors()
        {
            ISerializer serializer = new JsonNetSerializer();

            var badObject = new TestObjectWithException();

            string result = serializer.Serialize(badObject);

            Assert.Equal("{\"String\":\"A string\"}", result);
        }

        [Fact]
        public void RespectJsonPropertyOverrides()
        {
            ISerializer serializer = new JsonNetSerializer();

            var overrideObject = new TestObjectWithJsonAttributes();

            string result = serializer.Serialize(overrideObject);

            Assert.Equal("{\"meaningfullName\":\"A string\"}", result);
        }

        [Fact]
        public void RespectISerializableObjects()
        {
            ISerializer serializer = new JsonNetSerializer();

            var iSerializableObj = new TestObjectAsISerializable();

            var result = serializer.Serialize(iSerializableObj);

            Assert.Equal("{\"otherKey\":\"otherValue\"}", result);
        }

        [Fact]
        public void RegisterISerializationConverters()
        {
            var converter1 = new Mock<ISerializationConverter>();
            var converter2 = new Mock<ISerializationConverter>();

            ISerializer serializer = new JsonNetSerializer();

            serializer.RegisterSerializationConverters(new[]{converter1.Object, converter2.Object});
        }

        [Fact]
        public void RegisterEmptyCollectionOfISerializationConverters()
        {
            ISerializer serializer = new JsonNetSerializer();

            serializer.RegisterSerializationConverters(Enumerable.Empty<ISerializationConverter>());
        }

        [Fact]
        public void ThrowWhenRegisterNullCollectionOfISerializationConverters()
        {
            ISerializer serializer = new JsonNetSerializer();

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
