using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin.Types;

namespace Glimpse.Owin.Sample
{
    public class TimestampMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> next;

        public TimestampMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            await next(env);
            await new OwinResponse(env).WriteAsync("<em>After: " + DateTime.Now.ToLongTimeString() + "</em>");
        }
    }
}