using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin.Types;

namespace Glimpse.Owin.Middleware
{
    public class TailMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> innerNext;

        public TailMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            innerNext = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            await innerNext(environment);
            await new OwinResponse(environment).WriteAsync("<!-- Glimpse End @ " + DateTime.Now.ToLongTimeString() + "-->");
            //// this is where to end a request
        }
    }
}