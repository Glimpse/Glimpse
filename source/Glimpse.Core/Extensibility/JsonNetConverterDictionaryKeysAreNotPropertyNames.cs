using System;
using System.Collections;
using System.Globalization;
using Newtonsoft.Json;

namespace Glimpse.Core.Extensibility
{
    public class JsonNetConverterDictionaryKeysAreNotPropertyNames : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IDictionary).IsAssignableFrom(objectType);
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dictionary = (IDictionary)value;

            writer.WriteStartObject();

            foreach (DictionaryEntry entry in dictionary)
            {
                string key = Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
                writer.WritePropertyName(key);
                serializer.Serialize(writer, entry.Value);
            }

            writer.WriteEndObject();
        }
    }
}