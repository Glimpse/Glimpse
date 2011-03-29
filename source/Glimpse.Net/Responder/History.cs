using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class History : GlimpseResponder
    {
        [ImportingConstructor]
        public History(JavaScriptSerializer jsSerializer) : base(jsSerializer)
        {
        }

        public override string ResourceName
        {
            get { return "History"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration config)
        {
            if (!application.IsValidRequest(config, false, checkPath: false))
            {
                var data =
                    JsSerializer.Serialize(new {Error = true, Message = "You are not configured to access history."});
                JsonResponse(application, data);
                return;
            }

            var queue = application.Application[GlimpseConstants.JsonQueue] as Queue<GlimpseRequestMetadata>;
            if (queue != null)
            {
                var clientName = application.Request.QueryString[GlimpseConstants.ClientName];
                string data;

                if (string.IsNullOrEmpty(clientName))
                    data = JsSerializer.Serialize(queue);
                else
                {
                    var filteredQueue = from request in queue
                                        where request.ClientName.Equals(clientName)
                                        select request;
                    data = JsSerializer.Serialize(filteredQueue);
                }

                JsonResponse(application, data);
                return;
            }
            else
            {
                var data = JsSerializer.Serialize(new {Error = true, Message = "No history avalible."});
                JsonResponse(application, data);
                return;
            }
        }
    }
}