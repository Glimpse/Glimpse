using System;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class ContentTypeCollection : ConfigurationElementCollection
    {
        public ContentTypeCollection()
        {
            BaseAdd(new ContentType { Content = "text/html" });
            BaseAdd(new ContentType { Content = "application/json" });
        }

        public void Add(ContentType contentType)
        {
            BaseAdd(contentType);
        }

        public ContentType this[int index]
        {
            get { return BaseGet(index) as ContentType; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public bool Contains(string matchContentType)
        {
            foreach (ContentType ct in this)
            {
                if (ct.Content.Equals(matchContentType, StringComparison.InvariantCultureIgnoreCase)) return true;
            }

            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ContentType();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }
}