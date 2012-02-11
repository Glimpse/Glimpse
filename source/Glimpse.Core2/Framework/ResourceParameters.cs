using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class ResourceParameters
    {
        public static ResourceParameters None()
        {
            IDictionary<string, string> none = null;
            return new ResourceParameters(none);
        }

        public ResourceParameters(IDictionary<string, string> namedParameters)
        {
            NamedParameters = namedParameters;
        }

        public ResourceParameters(string[] orderedParameters)
        {
            OrderedParameters = orderedParameters;
        }

        internal IDictionary<string, string> NamedParameters { get; set; }

        internal string[] OrderedParameters { get; set; }

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