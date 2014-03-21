using System.Collections.Generic;

namespace Glimpse.WebApi.Core.Model
{
    public class ConfigurationModel
    {
        public IDictionary<string, object> Properties { get; set; }

        public IEnumerable<FilterModel> Filters { get; set; }

        public List<FormatterModel> Formatters { get; set; }

        public IEnumerable<ServiceModel> Services { get; set; }
    }
}
