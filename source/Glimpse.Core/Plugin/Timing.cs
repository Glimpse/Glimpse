/*using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Plugin
{
    [GlimpsePlugin]
    public class Timing:IGlimpsePlugin
    {
        public string Name
        {
            get { return "timeline"; }
        }

        public object GetData(HttpContextBase context)
        {
            var metadata = context.Items["TimerMetadata"] as TimerMetadata;

            return metadata;
        }

        public void SetupInit()
        {
            throw new System.NotImplementedException();
        }
    }
}*/