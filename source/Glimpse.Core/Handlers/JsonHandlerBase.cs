using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Handlers
{
    public abstract class JsonHandlerBase:IGlimpseHandler
    {
        private GlimpseSerializer Serializer { get; set; }

        protected JsonHandlerBase(GlimpseSerializer serializer)
        {
            Serializer = serializer;
        }

        public void ProcessRequest(HttpContextBase context)
        {
            //TODO: FIX ME, return 401 unauth
            /*if (!context.IsValidRequest(config, false, checkPath: false))
            {
                return new {Error = true, Message = "You are not configured to access history."};
            }*/

            var data = GetData(context);
            var dataString = Serializer.Serialize(data);

            var response = context.Response;
            response.Write(dataString);
            response.AddHeader("Content-Type", "application/json");
        }

        protected abstract object GetData(HttpContextBase context);
        public abstract string ResourceName{get;}
    }
}