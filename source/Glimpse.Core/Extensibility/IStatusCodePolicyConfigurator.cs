using System.Collections.Generic;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Represents a status code policy configurator
    /// </summary>
    public interface IStatusCodePolicyConfigurator
    {
        /// <summary>
        /// Gets the supported status codes
        /// </summary>
        IEnumerable<int> SupportedStatusCodes { get; }

        /// <summary>
        /// Gets a boolean indicating whether there are supported status codes
        /// </summary>
        bool ContainsSupportedStatusCodes { get; }

        /// <summary>
        /// Adds the given status codes to the list of supported status codes
        /// </summary>
        /// <param name="statusCodes">The status codes</param>
        void AddSupportedStatusCodes(IEnumerable<int> statusCodes);

        /// <summary>
        /// Adds the given status code to the list of supported status codes
        /// </summary>
        /// <param name="statusCode">The status code</param>
        void AddSupportedStatusCode(int statusCode);
    }
}