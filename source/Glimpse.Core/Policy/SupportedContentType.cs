using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Policy
{
    /// <summary>
    /// Represents a supported content type
    /// </summary>
    public class SupportedContentType
    {
        /// <summary>
        /// Gets the content type
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Gets the <see cref="RuntimePolicy"/> to apply on a match
        /// </summary>
        public RuntimePolicy RuntimePolicyToApply { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportedContentType" />
        /// </summary>
        /// <param name="contentType">The content type</param>
        /// <param name="runtimePolicyToApply">The <see cref="RuntimePolicy"/> to apply on a match</param>
        public SupportedContentType(string contentType, RuntimePolicy runtimePolicyToApply)
        {
            ContentType = contentType;
            RuntimePolicyToApply = runtimePolicyToApply;
        }
    }
}