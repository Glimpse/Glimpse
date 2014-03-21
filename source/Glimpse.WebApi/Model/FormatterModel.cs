using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.WebApi.Model
{
    public class FormatterModel
    {
        public string Name { get; set; }

        public List<string> SupportedMediaTypes { get; set; }

        public System.Collections.ObjectModel.Collection<System.Net.Http.Formatting.MediaTypeMapping> MediaTypeMappings { get; set; }
    }
}
