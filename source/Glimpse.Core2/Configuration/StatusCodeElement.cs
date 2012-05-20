using System.Configuration;

namespace Glimpse.Core2.Configuration
{
    public class StatusCodeElement:ConfigurationElement
    {
        [ConfigurationProperty("statusCode", IsRequired = true)]
        public int StatusCode
        {
            get { return (int) base["statusCode"]; }
            set { base["statusCode"] = value; }
        }
    }
}
