using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Glimpse.Net.Extensibility
{
    public interface IGlimpseConverter
    {
        IEnumerable<Type> SupportedTypes { get; }

        IDictionary<string, object> Serialize(object obj);
    }

    public class JsonConverterToIGlimpseConverterAdapter:JsonConverter
    {
        public IGlimpseConverter Converter { get; set; }

        public JsonConverterToIGlimpseConverterAdapter(IGlimpseConverter converter)
        {
            Converter = converter;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dict = Converter.Serialize(value);

            serializer.Serialize(writer, dict);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return Converter.SupportedTypes.Any(type => type == objectType);
        }
    }
}