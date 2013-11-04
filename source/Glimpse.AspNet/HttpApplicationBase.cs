using System;
using System.Web;

namespace Glimpse.AspNet
{
    public abstract class HttpApplicationBase
    {
        public abstract event EventHandler BeginRequest;

        public abstract event EventHandler PostAcquireRequestState;

        public abstract event EventHandler PostRequestHandlerExecute;
        
        public abstract event EventHandler PostReleaseRequestState;

        public abstract event EventHandler EndRequest;

        public abstract event EventHandler PreSendRequestHeaders;

        public abstract HttpApplicationStateBase Application { get; set; }
    }
}