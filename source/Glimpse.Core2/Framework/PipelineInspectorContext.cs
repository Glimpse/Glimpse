using System;
using System.Diagnostics.Contracts;
using Glimpse.Core2.Extensibility;

namespace Glimpse.Core2.Framework
{
    public class PipelineInspectorContext : IPipelineInspectorContext
    {
        public ILogger Logger { get; set; }
        public IProxyFactory ProxyFactory { get; set; }

        public PipelineInspectorContext(ILogger logger, IProxyFactory proxyFactory)
        {
            Contract.Requires<ArgumentNullException>(logger != null, "logger");
            Contract.Requires<ArgumentNullException>(proxyFactory != null, "proxyFactory");

            Logger = logger;
            ProxyFactory = proxyFactory;
        }
    }
}