using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node for collecting a list of status codes.
    /// </summary>
    /// <remarks>
    /// By default, <c>StatusCodeElementCollection</c>s contain three elements: <c>200</c>, <c>301</c> and <c>302</c>.
    /// </remarks>
    [ConfigurationCollection(typeof(StatusCodeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class StatusCodeElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeElementCollection" /> class with <c>200</c>, <c>301</c> and <c>302</c> added to the collection.
        /// </summary>
        public StatusCodeElementCollection()
        {
            BaseAdd(new StatusCodeElement { StatusCode = 200 });
            BaseAdd(new StatusCodeElement { StatusCode = 301 });
            BaseAdd(new StatusCodeElement { StatusCode = 302 });
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new StatusCodeElement();
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
            return ((StatusCodeElement)element).StatusCode;
        }
    }
}