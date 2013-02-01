using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// A class which contains all the parameters, whether named or ordered, needed to execute a resource.
    /// </summary>
    public class ResourceParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceParameters" /> class with a set of named parameters.
        /// </summary>
        /// <param name="namedParameters">The named parameters.</param>
        public ResourceParameters(IDictionary<string, string> namedParameters)
        {
            NamedParameters = namedParameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceParameters" /> class with a set of ordered parameters.
        /// </summary>
        /// <param name="orderedParameters">The ordered parameters.</param>
        public ResourceParameters(string[] orderedParameters)
        {
            OrderedParameters = orderedParameters;
        }

        internal IDictionary<string, string> NamedParameters { get; set; }

        internal string[] OrderedParameters { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="ResourceParameter"/> which contains no parameter values.
        /// </summary>
        /// <returns>An implementation of the null object pattern for <see cref="ResourceParameter"/>.</returns>
        public static ResourceParameters None()
        {
            return new ResourceParameters((IDictionary<string, string>)null);
        }

        /// <summary>
        /// Gets the parameters for a given <see cref="IResource"/>.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>The parameters for a given resource.</returns>
        public IDictionary<string, string> GetParametersFor(IResource resource)
        {
            if (NamedParameters != null)
            {
                return NamedParameters;
            }

            var result = new Dictionary<string, string>();

            if (OrderedParameters == null)
            {
                return result;
            }

            var index = 0;
            var parameterCount = OrderedParameters.Count();
            foreach (var parameter in resource.Parameters)
            {
                string value = null;

                if (index < parameterCount)
                {
                    value = OrderedParameters[index];
                }

                result.Add(parameter.Name, value);
                index++;
            }

            return result;
        }
    }
}