using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node for collecting a list of content types.
    /// </summary>
    /// <remarks>
    /// By default, <c>ContentTypeElementCollection</c>s contain two elements: <c>text/html</c> and <c>application/json</c>.
    /// </remarks>
    [ConfigurationCollection(typeof(ContentTypeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public sealed class ContentTypeElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeElementCollection" /> class with <c>text/html</c> and <c>application/json</c> added to the collection.
        /// </summary>
        public ContentTypeElementCollection()
        {
            BaseAdd(new ContentTypeElement { ContentType = @"text/html" });
            BaseAdd(new ContentTypeElement { ContentType = @"application/json" });
            BaseAdd(new ContentTypeElement { ContentType = @"text/plain" });
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ContentTypeElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ContentTypeElement)element).ContentType;
        }
    }
}