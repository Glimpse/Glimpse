using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Glimpse.Core.Extensibility
{
    public class JsonNetSerializer : ISerializer
    {
        public JsonNetSerializer(ILogger logger)
        {
            Logger = logger;

            Settings = new JsonSerializerSettings();
                           
            Settings.Error += (obj, args) =>
                                  {
                                      Logger.Error("Error serializing object.", args.ErrorContext.Error);
                                      args.ErrorContext.Handled = true;
                                  };
        }

        private ILogger Logger { get; set; }
        
        private JsonSerializerSettings Settings { get; set; }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, Settings);
        }

        public void RegisterSerializationConverters(IEnumerable<ISerializationConverter> converters)
        {
            if (converters == null)
            {
                throw new ArgumentNullException("converters");
            }

            var jsonConverters = Settings.Converters;

            jsonConverters.Clear();

            foreach (var converter in converters)
            {
                jsonConverters.Add(new JsonNetSerializationConverterAdapter(converter));
            }
        }
    }
}