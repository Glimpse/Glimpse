using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Glimpse.Core.Extensibility
{
    public class JsonNetSerializer : ISerializer
    {
        public JsonNetSerializer(ILogger logger)
        {
            Logger = logger;

            Settings = new JsonSerializerSettings();
            Settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            Settings.Converters.Add(new JsonNetConverterDictionaryKeysAreNotPropertyNames()); 
            Settings.Error += (obj, args) =>
                                  {
                                      Logger.Error("Error serializing object.", args.ErrorContext.Error);
                                      args.ErrorContext.Handled = true;
                                  };
        }

        private ILogger Logger { get; set; }
        
        private JsonSerializerSettings Settings { get; set; }

        public string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, Formatting.None, Settings);
        }

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