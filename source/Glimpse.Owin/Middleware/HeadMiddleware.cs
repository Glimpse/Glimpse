using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glimpse.Owin.Middleware
{
    public class HeadMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> next;
        private readonly MiddlewareManager manager;
        private readonly Type middlewareType;
        private readonly Guid builderId;

        public HeadMiddleware(Func<IDictionary<string, object>, Task> next, Type middlewareType, Guid builderId)
        {
            this.next = next;
            this.manager = MiddlewareManager.Instance;
            this.middlewareType = middlewareType;
            this.builderId = builderId;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            manager.Start(environment, middlewareType, builderId);
            await next(environment);
            manager.End(environment, middlewareType, builderId);
        }
    }
}
