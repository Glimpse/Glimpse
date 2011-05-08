using System.Configuration;

namespace Glimpse.Net.Configuration
{
    public class ContentType : ConfigurationElement
    {
        [ConfigurationProperty("contentType", IsRequired = true)]
        public string Content
        {
            get
            {
                return this["contentType"].ToString();
            }
            set { this["contentType"] = value; }
        }
    }
}