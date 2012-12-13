using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    public class ClientResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_client";

        public ClientResource()
        {
            ResourceName = "Glimpse.Core.glimpse.js";
            ResourceType = @"application/x-javascript";
            Name = InternalName;
        }

        public string Key 
        {
            get { return Name; }
        }
    }
}