using System.Configuration;

namespace Glimpse.Core.Configuration
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