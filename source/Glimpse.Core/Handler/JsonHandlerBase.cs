using System.Web;
using Newtonsoft.Json;

namespace Glimpse.Core.Handler
{
    public abstract class JsonHandlerBase:HandlerBase
    {
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        protected JsonHandlerBase(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
        }

        public override void Process(HttpContextBase context)
        {
            //TODO: FIX ME, return 401 unauth
            /*if (!context.IsValidRequest(config, false, checkPath: false))
            {
                return new {Error = true, Message = "You are not configured to access history."};
            }*/

            var data = GetData(context);
            var dataString = JsonConvert.SerializeObject(data, Formatting.None, JsonSerializerSettings);

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