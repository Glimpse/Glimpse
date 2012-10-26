namespace Glimpse.Core.Framework
{
    public interface IRequestMetadata
    {
        string RequestUri { get; }
        
        string RequestHttpMethod { get; }

        int ResponseStatusCode { get; }
        
        string ResponseContentType { get; }
        
        string IpAddress { get; }
        
        bool RequestIsAjax { get; }
        
        string ClientId { get; }
        
        string GetCookie(string name);
        
        string GetHttpHeader(string name);
    }
}