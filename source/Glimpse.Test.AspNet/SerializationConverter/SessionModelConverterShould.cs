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
            List<object> columns = GetFirstValueRowFromConvertedSessionModel(rows);

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
                            Value = testObject,
                            Type = testObject.GetType()
                        }
                };

            object rows = new SessionModelConverter().Convert(sessionModels);
            List<object> columns = GetFirstValueRowFromConvertedSessionModel(rows);

            Assert.Equal(columns[0], "Key1");
            Assert.Equal(columns[1], testObject);
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
                            Value = testObject,
                            Type = testObject.GetType()
                        }
                };

            object rows = new SessionModelConverter().Convert(sessionModels);
            List<object> columns = GetFirstValueRowFromConvertedSessionModel(rows);

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
                            Value = testObject,
                            Type = testObject.GetType()
                        }
                };

            object rows = new SessionModelConverter().Convert(sessionModels);
            List<object> columns = GetFirstValueRowFromConvertedSessionModel(rows);

            Assert.Equal(columns[0], "Key1");
            Assert.Equal(columns[1], "\\Non serializable type :(\\");
            Assert.Equal(columns[2], typeof(NonSerializableTestObject));
        }

        private static List<object> GetFirstValueRowFromConvertedSessionModel(object rows)
        {
            return ((IEnumerable<object>)((IEnumerable<object>)rows).ToList().Skip(1).First()).ToList(); // skip first row since it only contains column names
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
