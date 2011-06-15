using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Warning;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Glimpse.Core.Plumbing
{
    public class GlimpseSerializer
    {
        public JsonSerializerSettings Settings { get; set; }
        public Formatting DefaultFormatting { get; set; }

        public GlimpseSerializer()
        {
            Settings = new JsonSerializerSettings { ContractResolver = new GlimpseContractResolver() };
            Settings.Error += (obj, args) =>
            {
                var warnings = new HttpContextWrapper(HttpContext.Current).GetWarnings();
                warnings.Add(new SerializationWarning(args.ErrorContext.Error));
                args.ErrorContext.Handled = true;
            };

            Settings.Converters.Add(new JavaScriptDateTimeConverter());

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
