using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace Glimpse.Core2.Extensibility
{
    //TODO: Merge JSON.NET DLL - or remove dependency 
    public class JsonNetSerializer:ISerializer
    {
        private JsonSerializerSettings Settings { get; set; }

        public JsonNetSerializer()
        {
            Settings = new JsonSerializerSettings();
                           
            Settings.Error += (obj, args) =>
                                  {
                                      //Ignore errors
                                      //TODO: add logging here (args.ErrorContext.Error)
                                      args.ErrorContext.Handled = true;
                                  };
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, Settings);
        }

        public void RegisterSerializationConverters(IEnumerable<ISerializationConverter> converters)
        {
            Contract.Requires<ArgumentNullException>(converters != null, "converters");

            var jsonConverters = Settings.Converters;

            jsonConverters.Clear();

            foreach (var converter in converters)
            {
                jsonConverters.Add(new JsonNetSerializationConverterAdapter(converter));
            }
        }
    }
}