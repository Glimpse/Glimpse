using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of a parameter value provider. Typically used by <see cref="IClientScript"/>
    /// to specify which parameters it requires.
    /// </summary>
    public interface IParameterValueProvider
    {
        /// <summary>
        /// Specifies which parameters are needed by adding values to the provided defaults.
        /// </summary>
        /// <param name="defaults">The defaults.</param>
        void OverrideParameterValues(IDictionary<string, string> defaults);
    }
}