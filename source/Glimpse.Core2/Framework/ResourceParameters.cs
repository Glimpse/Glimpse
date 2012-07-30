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
            var parameterCount = OrderedParameters.Count();
            foreach (var parameter in resource.Parameters)
            {
                string value = null;

                if (index < parameterCount)
                    value = OrderedParameters[index];

                result.Add(parameter.Name, value);
                index++;
            }

            return result;
        }
    }
}