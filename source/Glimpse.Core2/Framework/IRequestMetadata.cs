namespace Glimpse.Core2.Framework
{
    //TODO: Add contracts to ensure these values are never null
    //TODO: Consider renaming, this is request AND response info
    public interface IRequestMetadata
    {
        string RequestUri { get; }
        string RequestHttpMethod { get; }
        string GlimpseClientName { get; }
        int ResponseStatusCode { get; }
        string ResponseContentType { get; }
        string IpAddress { get; }
        bool RequestIsAjax { get; }
    }
}