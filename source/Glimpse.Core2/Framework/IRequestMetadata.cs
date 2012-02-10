namespace Glimpse.Core2.Framework
{
    //TODO: Add contracts to ensure these values are never null
    public interface IRequestMetadata
    {
        string RequestUri { get; }
        string RequestHttpMethod { get; }
        string GetCookie(string name);
        string GetHttpHeader(string name);
        int ResponseStatusCode { get; }
        string ResponseContentType { get; }
        string IpAddress { get; }
        bool RequestIsAjax { get; }
    }
}