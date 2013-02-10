using Microsoft.Security.Application;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// The default <see cref="IHtmlEncoder"/> implementation of Glimpse, which leverages Microsoft's Web Protection Library.
    /// </summary>
    public class AntiXssEncoder : IHtmlEncoder
    {
        /// <summary>
        /// Encode a given value for use as in an Html attribute.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        /// Encoded result.
        /// </returns>
        public string HtmlAttributeEncode(string input)
        {
            return Encoder.HtmlAttributeEncode(input);
        }
    }
}
