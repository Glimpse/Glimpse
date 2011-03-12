using System.Configuration;

namespace Glimpse.Net.Configuration
{
    public class GlimpseConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("on", DefaultValue = "true", IsRequired = false)]
        public bool On
        {
            set { this["on"] = value; }
            get
            {
                bool result;
                bool.TryParse(this["on"].ToString(), out result);
                return result;
            }
        }

        [ConfigurationProperty("ipAddresses", IsRequired = true)] //TODO: Provide DefaultValue and make this not required
        public IpCollection IpAddresses
        {
            set { this["ipAddresses"] = value; }
            get
            {
                return this["ipAddresses"] as IpCollection;
            }
        }

        [ConfigurationProperty("contentTypes", IsRequired = true)] //TODO: Provide DefaultValue and make this not required
        public ContentTypeCollection ContentTypes
        {
            set { this["contentTypes"] = value; }
            get
            {
                return this["contentTypes"] as ContentTypeCollection;
            }
        }
    }
}