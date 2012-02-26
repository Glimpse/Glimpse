using System;
using Glimpse.Core2.Framework;

namespace Glimpse.Core2.Extensibility
{
    public class RuntimePolicyContext : IRuntimePolicyContext
    {
        public IRequestMetadata RequestMetadata { get; set; }

        public ILogger Logger { get; set; }

        private object RequestContext { get; set; }

        public RuntimePolicyContext(IRequestMetadata requestMetadata, ILogger logger, object requestContext)
        {
            if (requestMetadata == null) throw new ArgumentNullException("requestMetadata");
            if (logger == null) throw new ArgumentNullException("logger");
            if (requestContext == null) throw new ArgumentNullException("requestContext");
            
            RequestMetadata = requestMetadata;
            Logger = logger;
            RequestContext = requestContext;
        }

        public T GetRequestContext<T>() where T : class
        {
            return RequestContext as T;
        }

    }
}