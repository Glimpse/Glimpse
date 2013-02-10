using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node for creating a collection of types.
    /// </summary>
    [ConfigurationCollection(typeof(TypeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class TypeElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeElement();
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
            return ((TypeElement)element).Type;
        }
    }
}