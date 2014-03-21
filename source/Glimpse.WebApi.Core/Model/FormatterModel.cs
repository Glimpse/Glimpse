using System.Collections.Generic;

namespace Glimpse.WebApi.Core.Model
{
    public class FormatterModel
    {
        public string Name { get; set; }

        public List<string> SupportedMediaTypes { get; set; }

        public System.Collections.ObjectModel.Collection<System.Net.Http.Formatting.MediaTypeMapping> MediaTypeMappings { get; set; }
    }
}
