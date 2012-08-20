namespace Glimpse.Core.Extensibility
{
    public interface IHtmlEncoder
    {
        string HtmlAttributeEncode(string input);
    }
}