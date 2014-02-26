using System.Collections.Generic;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Represents a content type policy configurator
    /// </summary>
    public interface IContentTypePolicyConfigurator
    {
        /// <summary>
        /// Gets the supported content types
        /// </summary>
        IEnumerable<string> SupportedContentTypes { get; }

        /// <summary>
        /// Gets a boolean indicating whether there are supported content types
        /// </summary>
        bool ContainsSupportedContentTypes { get; }

        /// <summary>
        /// Adds the given content types to the list of supported content types
        /// </summary>
        /// <param name="contentTypes">The content types</param>
        void AddSupportedContentTypes(IEnumerable<string> contentTypes);

        /// <summary>
        /// Adds the given content type to the list of supported content types
        /// </summary>
        /// <param name="contentType">The content type</param>
        void AddSupportedContentType(string contentType);
    }
}