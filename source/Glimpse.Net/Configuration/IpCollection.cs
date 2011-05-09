using System.Configuration;
using System.Linq;
using System.Net;

namespace Glimpse.Net.Configuration
{
    public class IpCollection : ConfigurationElementCollection
    {
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

        public bool Contains(string matchIp)
        {
            foreach (IpAddress address in this)
            {
                if (address.Matches(matchIp)) return true;
            }
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new IpAddress();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IpAddress) element);
        }
    }
}