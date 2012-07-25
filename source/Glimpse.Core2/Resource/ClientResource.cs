namespace Glimpse.Core2.Resource
{
    public class Client : FileResource
    {
        internal const string InternalName = "client";

        public Client()
        {
            ResourceName = "Glimpse.Core2.glimpse.js";
            ResourceType = @"application/x-javascript";
            Name = InternalName;
        }
    }
}