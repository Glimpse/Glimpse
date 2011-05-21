using System.Configuration;

namespace Glimpse.WebForms.Configuration
{
    public class GlimpseConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("enabled", DefaultValue = "false", IsRequired = false)]
        public bool Enabled
        {
            set { this["enabled"] = value; }
            get
            {
                bool result = false; //matches the default above
                bool.TryParse(this["enabled"].ToString(), out result);
                return result;
            }
        }

        [ConfigurationProperty("rootUrlPath", DefaultValue = @"Glimpse", IsRequired = false)]
        public string RootUrlPath
        {
            set { this["rootUrlPath"] = value; }
            get { return this["rootUrlPath"].ToString(); }
        }

        [ConfigurationProperty("requestLimit", DefaultValue = "15", IsRequired = false)]
        public int RequestLimit
        {
            set { this["requestLimit"] = value; }
            get
            {
                int result = 0; //matches the default above
                int.TryParse(this["requestLimit"].ToString(), out result);
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

        [ConfigurationProperty("pluginBlacklist", IsRequired = true)] //TODO: Provide DefaultValue and make this not required
        public PluginBlacklistCollection PluginBlacklist
        {
            set { this["pluginBlacklist"] = value; }
            get
            {
                return this["pluginBlacklist"] as PluginBlacklistCollection;
            }
        }
    }
}