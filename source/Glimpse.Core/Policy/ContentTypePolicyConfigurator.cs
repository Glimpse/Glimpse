using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Implementation of an <see cref="IContentTypePolicyConfigurator" />
    /// </summary>
    public class ContentTypePolicyConfigurator : AddRemoveClearItemsConfigurator<SupportedContentType>, IContentTypePolicyConfigurator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypePolicyConfigurator" />
        /// </summary>
        public ContentTypePolicyConfigurator()
            : base("contentTypes", new SupportedContentTypeComparer())
        {
            AddSupportedContentType(new SupportedContentType("text/html", RuntimePolicy.On));
            AddSupportedContentType(new SupportedContentType("application/json", RuntimePolicy.On));
            AddSupportedContentType(new SupportedContentType("text/plain", RuntimePolicy.On));
        }

        /// <summary>
        /// Gets the supported content types
        /// </summary>
        public IEnumerable<SupportedContentType> SupportedContentTypes
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
        /// <param name="supportedContentTypes">The content types to support</param>
        public void AddSupportedContentTypes(IEnumerable<SupportedContentType> supportedContentTypes)
        {
            foreach (var supportedContentType in supportedContentTypes)
            {
                AddSupportedContentType(supportedContentType);
            }
        }

        /// <summary>
        /// Adds the given content type to the list of supported content types
        /// </summary>
        /// <param name="supportedContentType">The content type to support</param>
        public void AddSupportedContentType(SupportedContentType supportedContentType)
        {
            AddItem(supportedContentType);
        }

        /// <summary>
        /// Removes the given content type from the list of supported content types
        /// </summary>
        /// <param name="contentType">The content type to remove</param>
        public void RemoveSupportedContentType(string contentType)
        {
            RemoveItem(new SupportedContentType(contentType, RuntimePolicy.Off));
        }

        /// <summary>
        /// Creates an string representing a content type
        /// </summary>
        /// <param name="itemNode">The <see cref="XmlNode"/> from which a content type is created</param>
        /// <returns>A content type</returns>
        protected override SupportedContentType CreateItem(XmlNode itemNode)
        {
            if (itemNode != null && itemNode.Attributes != null)
            {
                XmlAttribute contentTypeAttribute = itemNode.Attributes["contentType"];
                if (contentTypeAttribute != null)
                {
                    string contentType = contentTypeAttribute.Value;
                    RuntimePolicy runtimePolicy = RuntimePolicy.On;

                    XmlAttribute runtimePolicyAttribute = itemNode.Attributes["runtimePolicy"];
                    if (runtimePolicyAttribute != null)
                    {
                        if (!Enum.TryParse(runtimePolicyAttribute.Value, out runtimePolicy))
                        {
                            throw new GlimpseException("'" + runtimePolicyAttribute.Value + "' is not a valid RuntimePolicy value");
                        }
                    }

                    return new SupportedContentType(contentType, runtimePolicy);
                }
            }

#warning CGI Add to resource file
            throw new GlimpseException("Could not find a 'contentType' attribute");
        }

        private class SupportedContentTypeComparer : IComparer<SupportedContentType>
        {
            public int Compare(SupportedContentType x, SupportedContentType y)
            {
                return string.Compare(x.ContentType, y.ContentType, System.StringComparison.Ordinal);
            }
        }
    }
}