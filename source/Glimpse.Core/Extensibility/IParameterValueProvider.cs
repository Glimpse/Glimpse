using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IParameterValueProvider</c> provides a mechanism to override or append Uri template parameter values when generating an <see cref="IDynamicClientScript"/>'s corresponding <see cref="IResource"/> Uri.
    /// </summary>
    public interface IParameterValueProvider
    {
        /// <summary>
        /// Used to override or append Uri template parameter values to the values required for the <see cref="IDynamicClientScript"/>.
        /// </summary>
        /// <param name="defaults">The default Uri template parameter values as defined by the Glimpse server.</param>
        void OverrideParameterValues(IDictionary<string, string> defaults);
    }
}