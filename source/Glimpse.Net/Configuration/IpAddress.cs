using System.Configuration;
using System.Text.RegularExpressions;

namespace Glimpse.Net.Configuration
{
    public class IpAddress : ConfigurationElement
    {
        [ConfigurationProperty("address", IsRequired = true)]
        [IpAddressFormat]
        public string Address
        {
            get
            {
                return this["address"].ToString();
            }
            set
            {
                this["address"] = value;
            }
        }

        [ConfigurationProperty("range")]
        public string Range
        {
            get 
            { 
                return this["range"].ToString();
            }
            set
            {
                this["range"] = value;
            }
        }

        public bool Matches(string matchIp)
        {
            if (!matchIp.IsValidIpAddress())
                return false;

            if (!string.IsNullOrWhiteSpace(Address))
            {
                if (Address == matchIp) return true;
            }

            if (!string.IsNullOrWhiteSpace(Range))
            {
                if (Regex.IsMatch(matchIp, Range)) return true;
            }

            return false;
        }

        // there's not a good way to specify that at least
        // one of many attributes is required; this will alert
        // if Address is not found unless Range exists
        protected override object OnRequiredPropertyNotFound(string name)
        {
            if (HasRequiredFields()) return null;

            return base.OnRequiredPropertyNotFound(name);
        }

        public bool HasRequiredFields()
        {
            return !string.IsNullOrWhiteSpace(Address) || !string.IsNullOrWhiteSpace(Range);
        }
    }
}