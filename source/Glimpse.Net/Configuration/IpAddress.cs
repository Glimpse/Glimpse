using System.Configuration;
using System.Net;

namespace Glimpse.WebForms.Configuration
{
    public class IpAddress : ConfigurationElement
    {
        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get
            {
                IPAddress result;

                if (IPAddress.TryParse(this["address"].ToString(), out result))
                    return result.ToString();

                throw new ConfigurationErrorsException("Invalid IP address found in glimpse\\ipFilter: " + this["address"]);
            }
            set { this["address"] = value; }
        }
    }
}