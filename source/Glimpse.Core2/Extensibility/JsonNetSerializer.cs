using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Glimpse.Core2.Extensibility
{
    //TODO: Merge JSON.NET DLL - or remove dependency 
    public class JsonNetSerializer:ISerializer
    {
        private JsonSerializerSettings Settings { get; set; }
        private ILogger Logger { get; set; }

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

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, Settings);
        }

        public void RegisterSerializationConverters(IEnumerable<ISerializationConverter> converters)
        {
            if (converters == null) throw new ArgumentNullException("converters");

            var jsonConverters = Settings.Converters;

            jsonConverters.Clear();

            foreach (var converter in converters)
            {
                jsonConverters.Add(new JsonNetSerializationConverterAdapter(converter));
            }
        }
    }
}