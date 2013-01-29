using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.Core.ClientScript
{
    /// <summary>
    /// The <see cref="IDynamicClientScript"/> implementation responsible for adding the Glimpse server configuration metadata <c>&lt;script&gt;</c> tag to a page response.
    /// </summary>
    public sealed class Metadata : IDynamicClientScript, IParameterValueProvider
    {
        /// <summary>
        /// Gets the sorting order in which a <c>&lt;script&gt;</c> tag will be injected a page response, relative to other implementations of <see cref="IClientScript" />.
        /// </summary>
        /// <value>
        /// Any value from the <see cref="ScriptOrder" /> enumeration.
        /// </value>
        /// <remarks>
        /// Multiple <see cref="IClientScript" />'s with the same <see cref="ScriptOrder" /> will be sorted in an indeterminate order.
        /// </remarks>
        public ScriptOrder Order
        {
            get { return ScriptOrder.RequestMetadataScript; }
        }

        /// <summary>
        /// Gets the name of the <see cref="IResource" /> to dynamically generate a Uri for.
        /// </summary>
        /// <returns>
        /// Name of the <see cref="IResource" /> to link to.
        /// </returns>
        /// <remarks>
        ///   <see cref="IResource" /> Uri generation is handled by implementations of <see cref="Glimpse.Core.Framework.ResourceEndpointConfiguration" />.
        /// </remarks>
        public string GetResourceName()
        {
            return MetadataResource.InternalName;
        }

        /// <summary>
        /// Used to override or append Uri template parameter values to the values required for the <see cref="IDynamicClientScript" />.
        /// </summary>
        /// <param name="defaults">The default Uri template parameter values as defined by the Glimpse server.</param>
        public void OverrideParameterValues(IDictionary<string, string> defaults)
        {
            defaults[ResourceParameter.Callback.Name] = "glimpse.data.initMetadata";
        }
    }
}