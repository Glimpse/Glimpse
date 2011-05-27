using System;
using System.Configuration;

namespace Glimpse.Core.Configuration
{
    public class Environment : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("authority", IsRequired = true)]
        public string Authority
        {
            get { return this["authority"].ToString(); }
            set { this["authority"] = value; }
        }

        public Uri Something(Uri requestUri)
        {
            var url = requestUri.ToString().Replace(requestUri.Authority, Authority);
            return new Uri(url);
        }
    }
}
