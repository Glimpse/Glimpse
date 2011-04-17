using System.ComponentModel.Composition;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;

namespace Glimpse.Net.Responder
{
    [GlimpseResponder]
    public class Popup : GlimpseResponder
    {
        [ImportingConstructor]
        public Popup(JavaScriptSerializer jsSerializer) : base(jsSerializer)
        {
        }

        public override string ResourceName
        {
            get { return "Popup"; }
        }

        public override void Respond(HttpApplication application, GlimpseConfiguration configuration)
        {
            var response = application.Response;

            ////<style>.glimpse-open { display:none; } </style>

            response.Write("<!DOCTYPE html><html><head><title>Glimpse - Popup</title><style type='text/css'>html{color:#000;background:#FFF;} body{margin:0;padding:0;} .glimpse-holder { position:relative !important; display: block !important; } .glimpse-buttons, .glimpse-open { display:none !important; } .glimpse-panel { overflow:visible !important; }</style></head><body><script src='/Scripts/jquery-1.4.4.min.js' type='text/javascript'></script><script type='text/javascript' id='glimpseClient' src='/Glimpse/glimpseClient.js'></script>");

            application.CompleteRequest();
        }
    }
}