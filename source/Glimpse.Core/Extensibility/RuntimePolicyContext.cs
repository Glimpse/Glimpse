using System;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    public class RuntimePolicyContext : IRuntimePolicyContext
    {
        public RuntimePolicyContext(IRequestMetadata requestMetadata, ILogger logger, object requestContext)
        {
            if (requestMetadata == null)
            {
                throw new ArgumentNullException("requestMetadata");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }
            
            RequestMetadata = requestMetadata;
            Logger = logger;
            RequestContext = requestContext;
        }

        public ILogger Logger { get; set; }
        
        public IRequestMetadata RequestMetadata { get; set; }
        
        private object RequestContext { get; set; }

        public T GetRequestContext<T>() where T : class
        {
            return RequestContext as T;
        }
    }
}