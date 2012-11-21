namespace Glimpse.Core.Resource
{
    public class ClientResource : FileResource
    {
        internal const string InternalName = "glimpse_client";

        public ClientResource()
        {
            ResourceName = "Glimpse.Core.glimpse.js";
            ResourceType = @"application/x-javascript";
            Name = InternalName;
        }
    }
}