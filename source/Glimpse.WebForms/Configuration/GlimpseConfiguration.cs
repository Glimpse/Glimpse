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
                bool result; //false which matches the default above
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
                int result;
                return !int.TryParse(this["requestLimit"].ToString(), out result) ? 15 : result;
            }
        }

        [ConfigurationProperty("ipAddresses", IsRequired = false)]
        public IpCollection IpAddresses
        {
            set { this["ipAddresses"] = value; }
            get
            {
                return this["ipAddresses"] as IpCollection;
            }
        }

        [ConfigurationProperty("contentTypes", IsRequired = false)]
        public ContentTypeCollection ContentTypes
        {
            set { this["contentTypes"] = value; }
            get
            {
                return this["contentTypes"] as ContentTypeCollection;
            }
        }

        [ConfigurationProperty("pluginBlacklist", IsRequired = false)]
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