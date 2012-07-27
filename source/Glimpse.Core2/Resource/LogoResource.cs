namespace Glimpse.Core2.Resource
{
    public class Logo:FileResource
    {
        internal const string InternalName = "glimpse-logo";

        public Logo()
        {
            ResourceName = "Glimpse.Core2.logo.png";
            ResourceType = "image/png";
            Name = InternalName;
        }  
    }
}