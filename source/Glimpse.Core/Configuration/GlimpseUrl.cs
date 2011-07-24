using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// A url in the UrlBlacklist collection/configuration section
    /// </summary>
    /// <remarks>Any request matching this regular expression is excluded from Glimpse</remarks>
    public class GlimpseUrl : ConfigurationElement
    {
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return this["url"].ToString();
            }
            set { this["url"] = value; }
        }
    }
}