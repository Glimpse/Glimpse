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
        IEnumerable<SupportedContentType> SupportedContentTypes { get; }

        /// <summary>
        /// Gets a boolean indicating whether there are supported content types
        /// </summary>
        bool ContainsSupportedContentTypes { get; }

        /// <summary>
        /// Adds the given content types to the list of supported content types
        /// </summary>
        /// <param name="supportedContentTypes">The content types to support</param>
        void AddSupportedContentTypes(IEnumerable<SupportedContentType> supportedContentTypes);

        /// <summary>
        /// Adds the given content type to the list of supported content types
        /// </summary>
        /// <param name="supportedContentType">The content type to support</param>
        void AddSupportedContentType(SupportedContentType supportedContentType);
    }
}