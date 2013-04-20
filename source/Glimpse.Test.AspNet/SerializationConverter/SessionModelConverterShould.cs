using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using Glimpse.AspNet.Model;
using Glimpse.AspNet.SerializationConverter;

using Xunit;

namespace Glimpse.Test.AspNet.SerializationConverter
{
    public class SessionModelConverterShould
    {
        [Fact]
        public void ConvertItemsWithNullType()
        {
            List<SessionModel> sessionModels = new List<SessionModel>
                {
                    new SessionModel
                        {
                            Key = "Key1",
                            Value = "Test Value"
                        }
                };

            object rows = new SessionModelConverter().Convert(sessionModels);
            List<object> columns = ((List<object>)((IEnumerable<object>)rows).Skip(1).First());

            Assert.Equal(columns[0], "Key1");
            Assert.Equal(columns[1], "Test Value");
            Assert.Equal(columns[2], null);
        }

        [Fact]
        public void ConvertSerializableItems()
        {
            SerializableTestObject testObject = new SerializableTestObject { TestProperty = "Test Value" };

            List<SessionModel> sessionModels = new List<SessionModel>
                {
                    new SessionModel
                        {
                            Key = "Key1",
                            Value = testObject.TestProperty,
                            Type = testObject.GetType()
                        }
                };

            object rows = new SessionModelConverter().Convert(sessionModels);
            List<object> columns = ((List<object>)((IEnumerable<object>)rows).Skip(1).First());

            Assert.Equal(columns[0], "Key1");
            Assert.Equal(columns[1], testObject.TestProperty);
            Assert.Equal(columns[2], typeof(SerializableTestObject));
        }

        [Fact]
        public void ConvertNonSerializableItemsWithToStringMethod()
        {
            NonSerializableTestObjectWithToString testObject = new NonSerializableTestObjectWithToString { TestProperty = "Test Value" };

            List<SessionModel> sessionModels = new List<SessionModel>
                {
                    new SessionModel
                        {
                            Key = "Key1",
                            Value = testObject.TestProperty,
                            Type = testObject.GetType()
                        }
                };

            object rows = new SessionModelConverter().Convert(sessionModels);
            List<object> columns = ((List<object>)((IEnumerable<object>)rows).Skip(1).First());

            Assert.Equal(columns[0], "Key1");
            Assert.Equal(columns[1], testObject.TestProperty);
            Assert.Equal(columns[2], typeof(NonSerializableTestObjectWithToString));
        }

        [Fact]
        public void FailToConvertNonSerializableItems()
        {
            NonSerializableTestObject testObject = new NonSerializableTestObject { TestProperty = "Test Value" };

            List<SessionModel> sessionModels = new List<SessionModel>
                {
                    new SessionModel
                        {
                            Key = "Key1",
                            Value = testObject.TestProperty,
                            Type = testObject.GetType()
                        }
                };

            object rows = new SessionModelConverter().Convert(sessionModels);
            List<object> columns = ((List<object>)((IEnumerable<object>)rows).Skip(1).First());

            Assert.Equal(columns[0], "Key1");
            Assert.Equal(columns[1], "\\Non serializable type :(\\");
            Assert.Equal(columns[2], typeof(NonSerializableTestObject));
        }
    }

    [Serializable]
    internal class SerializableTestObject : ISerializable
    {
        public string TestProperty { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TestProperty", this.TestProperty);
        }
    }

    internal class NonSerializableTestObject
    {
        public string TestProperty { get; set; }
    }

    internal class NonSerializableTestObjectWithToString : NonSerializableTestObject
    {
        public override string ToString()
        {
            return this.TestProperty;
        }
    }
}