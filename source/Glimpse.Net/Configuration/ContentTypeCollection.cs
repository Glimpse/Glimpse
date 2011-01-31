using System;
using System.Configuration;
using System.Net;

namespace Glimpse.Net.Configuration
{
    public class ContentTypeCollection : ConfigurationElementCollection
    {
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
            return ((ContentType)element);
        }
    }
}