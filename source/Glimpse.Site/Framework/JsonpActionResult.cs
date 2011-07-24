using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Glimpse.Site.Framework
{
    public class JsonpResult : JsonResult
    {
        public string CallbackFunction { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/javascript" : ContentType;

            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                HttpRequestBase request = context.HttpContext.Request;

                var callback = string.IsNullOrWhiteSpace(CallbackFunction) ? request.Params["callback"] : CallbackFunction;

#pragma warning disable 0618 // JavaScriptSerializer is no longer obsolete
                var serializer = new JavaScriptSerializer();
                response.Write(callback + "(" + serializer.Serialize(Data) + ");");
#pragma warning restore 0618
            }
        }
    }
}