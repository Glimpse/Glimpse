using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node representing an Http status code.
    /// </summary>
    public class StatusCodeElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// A list of ratified Http status codes in available in <see href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html">Section 10 of RFC 2616</see>, the Http version 1.1 specification.
        /// </value>
        /// <seealso href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html">Http 1.1 Specification</seealso>
        [ConfigurationProperty("statusCode", IsRequired = true)]
        public int StatusCode
        {
            get { return (int)base["statusCode"]; }
            set { base["statusCode"] = value; }
        }
    }
}
