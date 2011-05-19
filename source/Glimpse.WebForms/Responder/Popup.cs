using System.Web;
using Glimpse.WebForms.Configuration;

namespace Glimpse.WebForms.Responder
{
    [GlimpseResponder]
    public class Popup : GlimpseResponder
    {
        public override string ResourceName
        {
            get { return "Popup"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration configuration)
        {
            var response = application.Response;

            var path = VirtualPathUtility.ToAbsolute("~/", application.Context.Request.ApplicationPath);  //TODO need to centralize logic 
            var requestId = ""; //TODO need to get this id from somewhere

            response.Write("<!DOCTYPE html><html><head><title>Glimpse - Popup</title>");
            response.Write("<style type='text/css'>html{color:#000;background:#FFF;} body{margin:0;padding:0;} .glimpse-holder { position:relative !important; display: block !important; } .glimpse-popout, .glimpse-popout, .glimpse-terminate, .glimpse-open { display:none !important; } .glimpse-panel { overflow:visible !important; }</style>");
            response.Write("</head><body>");
            response.Write(string.Format(@"<script type='text/javascript' id='glimpseData' data-glimpse-requestID='{1}'>var glimpse, glimpsePath = '{0}'</script>", path, requestId));
            response.Write("<script src='/Scripts/jquery-1.4.4.min.js' type='text/javascript'></script><script type='text/javascript' id='glimpseClient' src='/Glimpse/glimpseClient.js'></script>");
            response.Write("</body></html>");

            application.CompleteRequest();
        }
    }
}