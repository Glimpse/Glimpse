namespace Glimpse.Core2.Resource
{
    public class ClientResource : FileResource
    {
        internal const string InternalName = "glimpse_client";

        public ClientResource()
        {
            ResourceName = "Glimpse.Core2.glimpse.js";
            ResourceType = @"application/x-javascript";
            Name = InternalName;
        }
    }
}