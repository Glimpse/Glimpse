using System.Configuration;

namespace Glimpse.Core.Configuration
{
    [ConfigurationCollection(typeof(RegexElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class RegexElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RegexElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RegexElement)element).Regex;
        }
    }
}