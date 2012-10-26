using System.Configuration;

namespace Glimpse.Core.Configuration
{
    [ConfigurationCollection(typeof(StatusCodeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class StatusCodeElementCollection : ConfigurationElementCollection
    {
        public StatusCodeElementCollection()
        {
            BaseAdd(new StatusCodeElement { StatusCode = 200 });
            BaseAdd(new StatusCodeElement { StatusCode = 301 });
            BaseAdd(new StatusCodeElement { StatusCode = 302 });
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new StatusCodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StatusCodeElement)element).StatusCode;
        }
    }
}