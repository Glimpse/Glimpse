using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Implementation of an <see cref="IContentTypePolicyConfigurator" />
    /// </summary>
    public class ContentTypePolicyConfigurator : AddRemoveClearItemsConfigurator<string>, IContentTypePolicyConfigurator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypePolicyConfigurator" />
        /// </summary>
        public ContentTypePolicyConfigurator()
            : base("contentTypes", new StringComparer())
        {
            AddSupportedContentType("text/html");
            AddSupportedContentType("application/json");
            AddSupportedContentType("text/plain");
        }

        /// <summary>
        /// Gets the supported content types
        /// </summary>
        public IEnumerable<string> SupportedContentTypes
        {
            get { return ConfiguredItems; }
        }

        /// <summary>
        /// Gets a boolean indicating whether there are supported content types
        /// </summary>
        public bool ContainsSupportedContentTypes
        {
            get { return ConfiguredItems.Count() != 0; }
        }

        /// <summary>
        /// Adds the given content types to the list of supported content types
        /// </summary>
        /// <param name="contentTypes">The content types</param>
        public void AddSupportedContentTypes(IEnumerable<string> contentTypes)
        {
            foreach (var contentType in contentTypes)
            {
                AddSupportedContentType(contentType);
            }
        }

        /// <summary>
        /// Adds the given content type to the list of supported content types
        /// </summary>
        /// <param name="contentType">The content type</param>
        public void AddSupportedContentType(string contentType)
        {
            AddItem(contentType);
        }

        /// <summary>
        /// Creates an string representing a content type
        /// </summary>
        /// <param name="itemNode">The <see cref="XmlNode"/> from which a content type is created</param>
        /// <returns>A content type</returns>
        protected override string CreateItem(XmlNode itemNode)
        {
            if (itemNode != null && itemNode.Attributes != null)
            {
                XmlAttribute contentTypeAttribute = itemNode.Attributes["contentType"];
                if (contentTypeAttribute != null)
                {
                    return contentTypeAttribute.Value;
                }
            }
#warning CGI Add to resource file
            throw new GlimpseException("Could not find a 'contentType' attribute");
        }

        private class StringComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return string.Compare(x, y, System.StringComparison.Ordinal);
            }
        }
    }
}