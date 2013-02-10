using System;
using System.ComponentModel;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// The <see cref="Enum"/> providing type safe access to cache directives as defined in <see href="http://www.w3.org/Protocols/rfc2616/rfc2616.html">Section 14.9 of RFC 2616</see>.
    /// </summary>
    /// <seealso href="http://www.w3.org/Protocols/rfc2616/rfc2616.html">Hypertext Transfer Protocol Protocol - HTTP/1.1</seealso>
    public enum CacheSetting
    {
        /// <summary>
        /// Indicates that the response may be cached by any cache.
        /// </summary>
        [Description("public")]
        Public,
        
        /// <summary>
        /// Indicates that the response is intended for a single user and must not be cached by a shared cache. 
        /// </summary>
        [Description("private")]
        Private,

        /// <summary>
        /// Indicates that the response should not be cached.
        /// </summary>
        [Description("no-cache")]
        NoCache,

        /// <summary>
        /// Indicates that the response should not be stored for sensitivity purposes.
        /// </summary>
        [Description("no-store")]
        NoStore,
    }
}