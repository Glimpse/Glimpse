using System.Web;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Handler
{
    public abstract class HandlerBase:IGlimpseHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            Process(new HttpContextWrapper(context));
        }

        public abstract bool IsReusable { get; }
        public abstract void Process(HttpContextBase context);
        public abstract string ResourceName { get; }
    }
}
