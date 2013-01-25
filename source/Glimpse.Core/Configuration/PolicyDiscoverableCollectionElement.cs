using System.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Policy;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node is a specialized <see cref="DiscoverableCollectionElement"/> used by <see cref="IRuntimePolicy"/>s and <see cref="IConfigurable"/>s to allow for end user configuration of runtime policies.
    /// </summary>
    public class PolicyDiscoverableCollectionElement : DiscoverableCollectionElement
    {
        /// <summary>
        /// Gets or sets a list of content types (aka media types or mime types).
        /// </summary>
        /// <remarks>
        /// Valid content type syntax is defined in <see href="http://www.ietf.org/rfc/rfc2046.txt">RFC 2046</see>. A <see href="http://www.iana.org/assignments/media-types">list of commonly used content types is available from IANA</see>.
        /// </remarks>
        /// <value>
        /// The <c>ContentTypes</c> list is used by <see cref="ContentTypePolicy"/> to filter invalid Glimpse responses.
        /// </value>
        [ConfigurationProperty("contentTypes")]
        public ContentTypeElementCollection ContentTypes
        {
            get { return (ContentTypeElementCollection)base["contentTypes"]; }
            set { base["contentTypes"] = value; }
        }

        /// <summary>
        /// Gets or sets a list of Http status codes.
        /// </summary>
        /// <remarks>
        /// A list of ratified Http status codes in available in <see href="http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html">Section 10 of RFC 2616</see>, the Http version 1.1 specification.
        /// </remarks>
        /// <value>
        /// The <c>StatusCodes</c> list is used by <see cref="StatusCodePolicy"/> to filter invalid Glimpse responses.
        /// </value>
        [ConfigurationProperty("statusCodes")]
        public StatusCodeElementCollection StatusCodes
        {
            get { return (StatusCodeElementCollection)base["statusCodes"]; }
            set { base["statusCodes"] = value; }
        }

        /// <summary>
        /// Gets or sets a list of Uris.
        /// </summary>
        /// <remarks>
        /// Each Uri in the <c>Uris</c> list must be a valid Uniform Resource Identifier, as defined by <see href="http://tools.ietf.org/html/rfc3986">RFC 3986</see>.
        /// In addition, "wildcard" <c>Uris</c> are supported via .NET regular expression syntax.
        /// </remarks>
        /// <value>
        /// The <c>Uris</c> list is used by <see cref="UriPolicy"/> to filter invalid Glimpse responses.
        /// </value>
        [ConfigurationProperty("uris")]
        public RegexElementCollection Uris
        {
            get { return (RegexElementCollection)base["uris"]; }
            set { base["uris"] = value; }
        }
    }
}