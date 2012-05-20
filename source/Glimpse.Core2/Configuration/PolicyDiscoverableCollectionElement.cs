using System.Configuration;

namespace Glimpse.Core2.Configuration
{
    public class PolicyDiscoverableCollectionElement:DiscoverableCollectionElement
    {
        [ConfigurationProperty("contentTypes")]
        public ContentTypeElementCollection ContentTypes
        {
            get { return (ContentTypeElementCollection) base["contentTypes"]; }
            set { base["contentTypes"] = value; }
        }

        [ConfigurationProperty("statusCodes")]
        public StatusCodeElementCollection StatusCodes
        {
            get { return (StatusCodeElementCollection) base["statusCodes"]; }
            set { base["statusCodes"] = value; }
        }

        [ConfigurationProperty("uris")]
        public RegexElementCollection Uris
        {
            get { return (RegexElementCollection) base["uris"]; }
            set { base["uris"] = value; }
        }
    }
}