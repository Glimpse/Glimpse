namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for a html encoder.
    /// </summary>
    public interface IHtmlEncoder
    {
        /// <summary>
        /// Encode a given value for use as a html attribute value.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Encoded result.</returns>
        string HtmlAttributeEncode(string input);
    }
}