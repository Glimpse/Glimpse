using System;
using System.Web;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class AspNetRequestResponseAdapter : IAspNetRequestResponseAdapter
    {
        public AspNetRequestResponseAdapter(HttpContextBase context, ILogger logger)
        {
            Context = context;
            Logger = logger;
        }

        public object RuntimeContext
        {
            get { return Context; }
        }

        public IRequestMetadata RequestMetadata
        {
            get { return new RequestMetadata(Context); }
        }

        internal HttpContextBase Context { get; set; }

        private ILogger Logger { get; set; }

        private bool SettingHttpResponseHeadersPrevented { get; set; }

        public void PreventSettingHttpResponseHeaders()
        {
            SettingHttpResponseHeadersPrevented = true;
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            if (!SettingHttpResponseHeadersPrevented)
            {
                try
                {
                    Context.Response.AppendHeader(name, value);
                }
                catch (Exception exception)
                {
                    Logger.Error("Exception setting Http response header '{0}' with value '{1}'.", exception, name, value);
                }
            }
            else
            {
                Logger.Error("Setting Http response header '{0}' with value '{1}' is not allowed anymore, headers are already sent.", name, value);
            }
        }

        public void SetHttpResponseStatusCode(int statusCode)
        {
            try
            {
                Context.Response.StatusCode = statusCode;
                Context.Response.StatusDescription = null;
            }
            catch (Exception exception)
            {
                Logger.Error("Exception setting Http status code with value '{0}'.", exception, statusCode);
            }
        }

        public void SetCookie(string name, string value)
        {
            try
            {
                Context.Response.Cookies.Add(new HttpCookie(name, value));
            }
            catch (Exception exception)
            {
                Logger.Error("Exception setting cookie '{0}' with value '{1}'.", exception, name, value);
            }
        }

        public void InjectHttpResponseBody(string htmlSnippet)
        {
            try
            {
                var response = Context.Response;
                response.Filter = new PreBodyTagInjectionStream(htmlSnippet, response.Filter, response.ContentEncoding, Context.Request != null ? Context.Request.RawUrl : null, Logger);
            }
            catch (Exception exception)
            {
                Logger.Error("Exception injecting Http response body with Html snippet '{0}'.", exception, htmlSnippet);
            }
        }

        public void WriteHttpResponse(byte[] content)
        {
            try
            {
                Context.Response.BinaryWrite(content);
            }
            catch (Exception exception)
            {
                Logger.Error("Exception writing Http response.", exception);
            }
        }

        public void WriteHttpResponse(string content)
        {
            try
            {
                Context.Response.Write(content);
            }
            catch (Exception exception)
            {
                Logger.Error("Exception writing Http response.", exception);
            }
        }
    }
}