using System.Configuration;
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
            IPAddress parsedAddress;

            if (!IPAddress.TryParse(matchIp, out parsedAddress))
                return false;

            foreach (IpAddress ip in this)
            {
                if (ip.Address == parsedAddress.ToString()) return true;
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