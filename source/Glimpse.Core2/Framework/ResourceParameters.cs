using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class ResourceParameters
    {
        public IDictionary<string, string> NamedParameters { private get; set; }

        public string[] OrderedParameters { private get; set; }

        public IDictionary<string,string> GetParametersFor(IResource resource)
        {
            if (NamedParameters != null)
                return NamedParameters;

            var result = new Dictionary<string, string>();

            if (OrderedParameters == null)
                return result;

            var index = 0;
            foreach (var key in resource.ParameterKeys)
            {
                var value = string.Empty;

                if (index < OrderedParameters.Count())
                    value = OrderedParameters[index];

                result.Add(key, value);
                index++;
            }

            return result;
        }
    }
}