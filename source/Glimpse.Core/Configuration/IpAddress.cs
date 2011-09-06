using System.Configuration;
using System.Net;
using LukeSkywalker.IPNetwork;

namespace Glimpse.Core.Configuration
{
    public class IpAddress : ConfigurationElement
    {
        [ConfigurationProperty("address")]
        public string Address
        {
            get
            {
                if(string.IsNullOrEmpty(this["address"].ToString()))
                    return null;

                IPAddress result;

                if (IPAddress.TryParse(this["address"].ToString(), out result))
                    return result.ToString();

                throw new ConfigurationErrorsException("Invalid IP address found in glimpse\\ipAddresses: " + this["address"]);
            }
            set { this["address"] = value; }
        }
        
        [ConfigurationProperty("address-range")]
        public string AddressRange
        {
            get
            {
                if(string.IsNullOrEmpty(this["address-range"].ToString()))
                    return null;

                IPNetwork result;

                if (IPNetwork.TryParse(this["address-range"].ToString(), out result))
                    return result.ToString();

                throw new ConfigurationErrorsException("Invalid IP address range found in glimpse\\ipAddresses: " + this["address-range"]);
            }
            set { this["address-range"] = value; }
        }
    }
}