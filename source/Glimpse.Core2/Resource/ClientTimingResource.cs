namespace Glimpse.Core2.Resource
{
    public class ClientTimingResource : FileResource
    {
        internal const string InternalName = "glimpse_client_timing";

        public ClientTimingResource()
        {
            ResourceName = "Glimpse.Core2.clientTiming.js";
            ResourceType = @"application/x-javascript";
            Name = InternalName;
        }
    }
}