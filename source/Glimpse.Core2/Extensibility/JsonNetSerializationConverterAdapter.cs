using System;
using System.Linq;
using Newtonsoft.Json;

namespace Glimpse.Core2.Extensibility
{
    public class JsonNetSerializationConverterAdapter : JsonConverter
    {
            private ISerializationConverter Converter { get; set; }

            public JsonNetSerializationConverterAdapter(ISerializationConverter converter)
            {
                Converter = converter;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var dict = Converter.Convert(value);

                serializer.Serialize(writer, dict);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotSupportedException();
            }

            public override bool CanConvert(Type objectType)
            {
                return Converter.SupportedTypes.Any(type => type.IsAssignableFrom(objectType));
            }
    }
}