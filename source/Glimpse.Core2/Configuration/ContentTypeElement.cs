using System.Configuration;

namespace Glimpse.Core2.Configuration
{
    public class ContentTypeElement:ConfigurationElement
    {
        [ConfigurationProperty("contentType", IsRequired = true)]
        public string ContentType
        {
            get { return (string) base["contentType"]; }
            set { base["contentType"] = value; }
        }
    }
}