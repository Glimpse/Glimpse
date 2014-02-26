using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Represents a uri policy configurator
    /// </summary>
    public interface IUriPolicyConfigurator
    {
        /// <summary>
        /// Gets the uri patterns that will be ignored
        /// </summary>
        IEnumerable<Regex> UriPatternsToIgnore { get; }

        /// <summary>
        /// Gets a boolean indicating whether there are uri patterns that will be ignored
        /// </summary>  
        bool ContainsUriPatternsToIgnore { get; }

        /// <summary>
        /// Adds the given uri patterns to the the list of uri patterns to ignore
        /// </summary>
        /// <param name="uriPatternsToIgnore">The uri patterns to ignore</param>
        void AddSupportedStatusCodes(IEnumerable<string> uriPatternsToIgnore);

        /// <summary>
        /// Adds the given uri pattern to the list of uri patterns that will be ignored
        /// </summary>
        /// <param name="uriPattern">The uri pattern</param>
        void AddUriPatternToIgnore(string uriPattern);
    }
}