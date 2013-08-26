using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Glimpse.Core.Framework;
using Owin;
using Owin.Types;

namespace Glimpse.Owin.Middleware
{
    public class HeadMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> innerNext;
        private readonly IAppBuilder app;

        public HeadMiddleware(Func<IDictionary<string, object>, Task> next, IAppBuilder app)
        {
            innerNext = next;
            this.app = app;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var locator = new OwinServiceLocator(environment, app);
            var factory = new Factory(locator);
            var glimpse = factory.InstantiateRuntime();

            glimpse.BeginRequest();

            // this where to check to see if this should handle the request directly (ala glimpse.axd)
            // this is where to start a new request for processing
            await new OwinResponse(environment).WriteAsync("<!-- Glimpse Start @ " + DateTime.Now.ToLongTimeString() + "-->");
            await innerNext(environment);
        }
    }
}