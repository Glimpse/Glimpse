namespace Glimpse.Core2.Framework
{
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
        string ClientId { get; }
    }
}