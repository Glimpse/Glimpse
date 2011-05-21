using System.Collections.Generic;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class PluginBlacklistCollection : ConfigurationElementCollection
    {
        public GlimpsePlugin this[int index]
        {
            get { return BaseGet(index) as GlimpsePlugin; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(GlimpsePlugin plugin)
        {
            BaseAdd(plugin);
        }

        public IEnumerable<string> TypeNames()
        {
            foreach (GlimpsePlugin plugin in this)
            {
                yield return plugin.TypeName;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new GlimpsePlugin();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }
}
