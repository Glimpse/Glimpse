using System.Web;
using Glimpse.Core.Extensibility;
using Newtonsoft.Json;

namespace Glimpse.Core.Handler
{
    public abstract class JsonHandlerBase:IGlimpseHandler
    {
        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        protected JsonHandlerBase(JsonSerializerSettings jsonSerializerSettings)
        {
            JsonSerializerSettings = jsonSerializerSettings;
        }

        public void ProcessRequest(HttpContext context)
        {
            //TODO: FIX ME, return 401 unauth
            /*if (!application.IsValidRequest(config, false, checkPath: false))
            {
                return new {Error = true, Message = "You are not configured to access history."};
            }*/

            var data = GetData(context);
            var dataString = JsonConvert.SerializeObject(data, Formatting.None, JsonSerializerSettings);

            var response = context.Response;
            response.Write(dataString);
            response.AddHeader("Content-Type", "application/json");
        }

        protected abstract object GetData(HttpContext context);
        public abstract string ResourceName { get;}

        public bool IsReusable
        {
            get { return true; }
        }
    }
}