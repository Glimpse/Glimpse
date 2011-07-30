using System.Configuration;
using System.Net;

namespace Glimpse.Core.Configuration
{
    public class IpCollection : ConfigurationElementCollection
    {
        public IpCollection()
        {
            BaseAdd(new IpAddress{Address = "127.0.0.1"});//IPv4
            BaseAdd(new IpAddress{Address = "::1"});//IPv6
        }

        public IpAddress this[int index]
        {
            get { return BaseGet(index) as IpAddress; }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(IpAddress address)
        {
            BaseAdd(address);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new IpAddress();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
    }
}