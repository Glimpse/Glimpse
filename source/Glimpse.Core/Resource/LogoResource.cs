namespace Glimpse.Core.Resource
{
    public class LogoResource : FileResource
    {
        internal const string InternalName = "glimpse_logo";

        public LogoResource()
        {
            ResourceName = "Glimpse.Core.logo.png";
            ResourceType = "image/png";
            Name = InternalName;
        }  
    }
}