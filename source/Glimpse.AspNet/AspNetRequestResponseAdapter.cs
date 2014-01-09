using System;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Web;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class AspNetRequestResponseAdapter : IRequestResponseAdapter
    {
        private HttpContextBase context;

        public AspNetRequestResponseAdapter(HttpContextBase context, ILogger logger)
        {
            Context = context;
            Logger = logger;
        }

        private readonly static bool AsyncSupportDisabled = Convert.ToBoolean(ConfigurationManager.AppSettings["Glimpse:DisableAsyncSupport"]);

        public IDataStore HttpRequestStore
        {
            get { return new DictionaryDataStoreAdapter(Context.Items); }
        }

        public IDataStore HttpServerStore
        {
            get { return new HttpApplicationStateBaseDataStoreAdapter(Context.Application); }
        }

        public object RuntimeContext
        {
            get { return Context; }
        }

        public IRequestMetadata RequestMetadata
        {
            get { return new RequestMetadata(Context); }
        }

        // V2Merge: We may be able to get away with a simple property in the future
        internal HttpContextBase Context
        {
            get { return context ?? TryGetOrCaptureLogicalContext(); }
            set { context = value; }
        }

        private ILogger Logger { get; set; }

        private static HttpContextBase TryGetOrCaptureLogicalContext()
        {
            if (AsyncSupportDisabled)
            {
                return new HttpContextWrapper(HttpContext.Current);
            }

            if (HttpContext.Current == null)
                return AntiSerializationWrapper<HttpContextBase>.Unwrap(CallContext.LogicalGetData("Glimpse.HttpContext"));

            var context = new HttpContextWrapper(HttpContext.Current);
            CallContext.LogicalSetData("Glimpse.HttpContext", new AntiSerializationWrapper<HttpContextBase>(context));

            return context;
        }

        public void SetHttpResponseHeader(string name, string value)
        {
            if (!Context.HeadersSent())
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
                response.Filter = new PreBodyTagFilter(htmlSnippet, response.Filter, response.ContentEncoding, Context.Request != null ? Context.Request.RawUrl : null, Logger);
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

        [Serializable]
        private struct AntiSerializationWrapper<T> : ISerializable
        {
            private readonly T value;

            public AntiSerializationWrapper(T value)
            {
                this.value = value;
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                throw new NotSupportedException(
                    "Some environments conflict with current Glimpse async support. " +
                    "Please set Glimpse:DisableAsyncSupport = true in Web.config, or see https://github.com/Glimpse/Glimpse/issues/632 for more details.");
            }

            public static T Unwrap(object wrapper)
            {
                if (ReferenceEquals(wrapper, null))
                    return default(T);

                return ((AntiSerializationWrapper<T>)wrapper).value;
            }
        }
    }
}