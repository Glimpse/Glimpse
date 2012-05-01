using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Sanitizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Glimpse.Core.Plumbing
{
    public class GlimpseSerializer
    {
        public JsonSerializerSettings Settings { get; set; }
        public Formatting DefaultFormatting { get; set; }
        public IGlimpseLogger Logger { get; set; }

        public GlimpseSerializer(IGlimpseFactory factory)
        {
            Logger = factory.CreateLogger();

            Settings = new JsonSerializerSettings { ContractResolver = new GlimpseContractResolver() };
            Settings.Error += (obj, args) =>
            {
                Logger.Warn("Serializer error", args.ErrorContext.Error);
                args.ErrorContext.Handled = true;
            };

            Settings.Converters.Add(new JavaScriptDateTimeConverter());
            Settings.Converters.Add(new CSharpTypenameConverter());

            DefaultFormatting = Formatting.None;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, DefaultFormatting, Settings);
        }

        public void AddConverters(IEnumerable<IGlimpseConverter> glimpseConverters)
        {
            var converters = Settings.Converters;
            foreach (var converter in glimpseConverters)
            {
                converters.Add(new JsonConverterToIGlimpseConverterAdapter(converter));
            }
        }
    }
}
