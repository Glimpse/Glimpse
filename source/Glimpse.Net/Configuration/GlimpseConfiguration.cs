using System.Configuration;

namespace Glimpse.Net.Configuration
{
    public class GlimpseConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("on", DefaultValue = "false", IsRequired = false)]
        public bool On
        {
            set { this["on"] = value; }
            get
            {
                bool result = false; //matches the default above
                bool.TryParse(this["on"].ToString(), out result);
                return result;
            }
        }

        [ConfigurationProperty("pluginPath", DefaultValue = @"\", IsRequired = false)]
        public string PluginPath
        {
            set { this["pluginPath"] = value; }
            get { return this["pluginPath"].ToString(); }
        }

        [ConfigurationProperty("saveRequestCount", DefaultValue = "0", IsRequired = false)]
        public int SaveRequestCount
        {
            set { this["saveRequestCount"] = value; }
            get
            {
                int result = 0; //matches the default above
                int.TryParse(this["saveRequestCount"].ToString(), out result);
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