using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;

namespace Glimpse.Core.ClientScript
{
    /// <summary>
    /// The <see cref="IDynamicClientScript"/> implementation responsible for adding the Glimpse JavaScript client <c>&lt;script&gt;</c> tag to a page response.
    /// </summary>
    public sealed class ClientPre : IDynamicClientScript
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
            get { return ScriptOrder.IncludeBeforeClientInterfaceScript; }
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
            return ClientPreResource.InternalName;
        }
    }
}