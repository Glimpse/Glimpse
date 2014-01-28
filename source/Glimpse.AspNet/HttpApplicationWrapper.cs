using System;
using System.Web;

namespace Glimpse.AspNet
{
    public class HttpApplicationWrapper : HttpApplicationBase
    {
        private HttpApplicationStateBase httpApplicationStateBase;

        public HttpApplicationWrapper(HttpApplication httpApplication)
        {
            HttpApplication = httpApplication;
        }

        public override event EventHandler BeginRequest 
        {
            add { HttpApplication.BeginRequest += value; }
            remove { HttpApplication.BeginRequest -= value; }
        }

        public override event EventHandler PostAcquireRequestState
        {
            add { HttpApplication.PostAcquireRequestState += value; }
            remove { HttpApplication.PostAcquireRequestState -= value; }
        }

        public override event EventHandler PostRequestHandlerExecute
        {
            add { HttpApplication.PostRequestHandlerExecute += value; }
            remove { HttpApplication.PostRequestHandlerExecute -= value; }
        }

        public override event EventHandler PostReleaseRequestState
        {
            add { HttpApplication.PostReleaseRequestState += value; }
            remove { HttpApplication.PostReleaseRequestState -= value; }
        }

        public override event EventHandler EndRequest
        {
            add { HttpApplication.EndRequest += value; }
            remove { HttpApplication.EndRequest -= value; }
        }

        public override event EventHandler PreSendRequestHeaders
        {
            add { HttpApplication.PreSendRequestHeaders += value; }
            remove { HttpApplication.PreSendRequestHeaders -= value; }
        }

        public override HttpApplicationStateBase Application
        {
            get
            {
                return httpApplicationStateBase ??
                       (httpApplicationStateBase = new HttpApplicationStateWrapper(HttpApplication.Application));
            }

            set
            {
                httpApplicationStateBase = value;
            }
        }

        private HttpApplication HttpApplication { get; set; }
    }
}