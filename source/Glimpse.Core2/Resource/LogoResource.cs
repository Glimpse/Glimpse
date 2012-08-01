namespace Glimpse.Core2.Resource
{
    public class LogoResource:FileResource
    {
        internal const string InternalName = "glimpse_logo";

        public LogoResource()
        {
            ResourceName = "Glimpse.Core2.logo.png";
            ResourceType = "image/png";
            Name = InternalName;
        }  
    }
}