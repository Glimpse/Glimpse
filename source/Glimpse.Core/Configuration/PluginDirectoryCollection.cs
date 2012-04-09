using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Glimpse.Core.Configuration
{
    public class PluginDirectoryCollection : ConfigurationElementCollection
    {
        public PluginDirectoryCollection()
        {
            BaseAdd(new PluginDirectory { Directory = "bin" });
        }

        public PluginDirectory this[int index]
        {
            get { return BaseGet(index) as PluginDirectory; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(PluginDirectory address)
        {
            BaseAdd(address);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PluginDirectory();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }

        public IEnumerable<string> DirectoryNames 
        {
            get
            {
                foreach (PluginDirectory directory in this)
                {
                    yield return directory.ToString();
                }
            }
        }
    }
}
