using System;
using System.Linq;
using Newtonsoft.Json;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An adapter which converts Glimpse's <see cref="ISerializationConverter"/> to Json.Net <see cref="JsonConverter"/>.
    /// </summary>
    public class JsonNetSerializationConverterAdapter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetSerializationConverterAdapter" /> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        public JsonNetSerializationConverterAdapter(ISerializationConverter converter)
            {
                Converter = converter;
            }

        private ISerializationConverter Converter { get; set; }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dict = Converter.Convert(value);

            serializer.Serialize(writer, dict);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        /// <exception cref="System.NotSupportedException">An exception is thrown if this method is called because it it not supported by Glimpse.</exception>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        ///   <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return Converter.SupportedTypes.Any(type => type.IsAssignableFrom(objectType));
        }
    }
}