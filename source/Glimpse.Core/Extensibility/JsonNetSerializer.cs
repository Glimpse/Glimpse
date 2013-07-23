using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// An implementation of <see cref="ISerializer"/> which leverages Json.Net.
    /// </summary>
    public class JsonNetSerializer : ISerializer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetSerializer" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public JsonNetSerializer(ILogger logger)
        {
            Logger = logger;

            Settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters =
                {
                    new JsonNetConverterDictionaryKeysAreNotPropertyNames(),
                },
            };

            Settings.Error += (obj, args) =>
                                  {
                                      Logger.Error("Error serializing object.", args.ErrorContext.Error);
                                      args.ErrorContext.Handled = true;
                                  };
        }

        private ILogger Logger { get; set; }
        
        private JsonSerializerSettings Settings { get; set; }

        /// <summary>
        /// Serializes the specified object to JSON.
        /// </summary>
        /// <param name="target">The target to be Serialized.</param>
        /// <returns>
        /// Serialized object.
        /// </returns>
        public string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, Formatting.None, Settings);
        }

        /// <summary>
        /// Registers a collection of serialization converters which can conduct custom
        /// transformations on given types when processed.
        /// </summary>
        /// <param name="converters">The converters.</param>
        /// <exception cref="System.ArgumentNullException">Throws an exception if <paramref name="converters"/> is <c>null</c>.</exception>
        public void RegisterSerializationConverters(IEnumerable<ISerializationConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException("converters");
            }

            var jsonConverters = Settings.Converters; 
            foreach (var converter in converters)
            {
                jsonConverters.Add(new JsonNetSerializationConverterAdapter(converter));
            }
        }
    }
}