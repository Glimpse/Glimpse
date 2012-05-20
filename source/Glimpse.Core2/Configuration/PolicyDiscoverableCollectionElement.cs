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

        //TODO: Add nodes for allowed StatusCodes w/ defaults
        //TODO: Add nodes for Uri RegEx's w/ defaults
    }
}