using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(requestMetadata != null, "requestMetadata");
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Requires<ArgumentNullException>(requestContext != null, "requestContext");

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