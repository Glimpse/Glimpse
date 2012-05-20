using System.Configuration;

namespace Glimpse.Core2.Configuration
{
    [ConfigurationCollection(typeof(ContentTypeElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ContentTypeElementCollection:ConfigurationElementCollection
    {
        public ContentTypeElementCollection()
        {
            base.BaseAdd(new ContentTypeElement { ContentType = @"text/html"});
            base.BaseAdd(new ContentTypeElement { ContentType = @"application/json" });
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ContentTypeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ContentTypeElement) element).ContentType;
        }
    }
}