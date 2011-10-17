namespace Glimpse.Core2
{
    public class RequestMetadata
    {
        public string Uri { get; set; }
        public string HttpMethod { get; set; }
        //TODO: Add other properties to be provided by framework implementation
        //public int ResponseStatusCode { get; set; }
        //public string ResponseContentType { get; set; }
        //public string IpAddress { get; set; }
    }
}