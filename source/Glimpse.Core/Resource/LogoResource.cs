using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Resource
{
    public class LogoResource : FileResource, IKey
    {
        internal const string InternalName = "glimpse_logo";

        public LogoResource()
        {
            ResourceName = "Glimpse.Core.logo.png";
            ResourceType = "image/png";
            Name = InternalName;
        }

        public string Key
        {
            get { return Name; }
        }
    }
}