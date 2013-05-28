using Glimpse.Core.Extensibility;

namespace MvcMusicStore
{
    public class SampleDisplay : Glimpse.Core.Extensibility.IDisplay
    {
        public string Name 
        {
            get { return "Sample"; }
        }

        public object GetData(ITabContext context)
        {
            return 77;
        }
    }
}