using System.Configuration;

namespace Glimpse.Core.Configuration
{
    [ConfigurationCollection(typeof(TypeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class TypeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TypeElement)element).Type;
        }
    }
}