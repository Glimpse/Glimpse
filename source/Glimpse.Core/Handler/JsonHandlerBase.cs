using System.ComponentModel.Composition;
using System.Web;
using Glimpse.Core.Plumbing;
using Newtonsoft.Json;

namespace Glimpse.Core.Handler
{
    public abstract class JsonHandlerBase:HandlerBase
    {
        private GlimpseSerializer Serializer { get; set; }

        protected JsonHandlerBase(GlimpseSerializer serializer)
        {
            Serializer = serializer;
        }

        public override void Process(HttpContextBase context)
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
        
        public override bool IsReusable
        {
            get { return true; }
        }
    }
}