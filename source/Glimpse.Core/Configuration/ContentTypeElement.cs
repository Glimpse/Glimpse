using System.Configuration;

namespace Glimpse.Core.Configuration
{
    /// <summary>
    /// The Glimpse configuration node for representing a content type.
    /// </summary>
    public class ContentTypeElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <value>
        /// Valid content type syntax is defined in <see href="http://www.ietf.org/rfc/rfc2046.txt">RFC 2046</see>. A <see href="http://www.iana.org/assignments/media-types">list of commonly used content types is available from IANA</see>.
        /// </value>
        /// <seealso href="http://www.ietf.org/rfc/rfc2046.txt">Multipurpose Internet Mail Extensions(MIME) Part Two: Media Types</seealso>
        /// <seealso href="http://www.iana.org/assignments/media-types">IANA MIME Media Types</seealso>
        [ConfigurationProperty("contentType", IsRequired = true)]
        public string ContentType
        {
            get { return (string)base["contentType"]; }
            set { base["contentType"] = value; }
        }
    }
}