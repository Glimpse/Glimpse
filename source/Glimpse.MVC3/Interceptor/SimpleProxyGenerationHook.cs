using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.Interceptor
{
    internal class SimpleProxyGenerationHook:BaseProxyGenerationHook
    {
        internal string[] Methods { get; set; }

        public SimpleProxyGenerationHook(IGlimpseLogger logger, params string[] methods) : base(logger)
        {
            Methods = methods;
        }

        public override IEnumerable<string> GetMethodNames()
        {
            return Methods.AsEnumerable();
        }
    }
}