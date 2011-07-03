using System.Collections.Generic;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class UrlBlacklistCollection : ConfigurationElementCollection
    {
        public GlimpseUrl this[int index]
        {
            get { return BaseGet(index) as GlimpseUrl; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(GlimpseUrl url)
        {
            BaseAdd(url);
        }

        public IEnumerable<string> Urls()
        {
            foreach (GlimpseUrl url in this)
            {
                yield return url.Url;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new GlimpseUrl();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }
}
