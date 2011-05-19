using System.Configuration;

namespace Glimpse.WebForms.Configuration
{
    public class GlimpsePlugin : ConfigurationElement
    {
        [ConfigurationProperty("plugin", IsRequired = true)]
        public string TypeName
        {
            get
            {
                return this["plugin"].ToString();
            }
            set { this["plugin"] = value; }
        }
    }
}