using System;
using System.Collections.Generic;
using Glimpse.AspNet5.Middleware;
using Microsoft.AspNet.Builder;

namespace Glimpse.AspNet5
{
    public class ApplicationBuilder : IApplicationBuilder
    {
        private readonly IApplicationBuilder innerBuilder;
        private readonly static IDictionary<string, object> ApplicationStore = new Dictionary<string, object>();
        private readonly Guid builderId;
        private readonly MiddlewareManager manager;

        public ApplicationBuilder(IApplicationBuilder app)
        {
            innerBuilder = app;

            manager = MiddlewareManager.Instance;
            builderId = Guid.NewGuid();

            innerBuilder.Use(next => new GlimpseMiddleware(next, ApplicationStore).Invoke); // This is the earliest we can add middleware
        }
         
        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            manager.Register(builderId, middleware.Target.GetType());

            innerBuilder.Use(next => new HeadMiddleware(next, next.Target.GetType(), builderId).Invoke);
            innerBuilder.Use(middleware);

            return this;
        }

        public IApplicationBuilder New()
        {
            return new ApplicationBuilder(innerBuilder.New());
        }

        public RequestDelegate Build()
        {
            return innerBuilder.Build();
        }

        public IServiceProvider ApplicationServices 
        {
            get { return innerBuilder.ApplicationServices; }
            set { innerBuilder.ApplicationServices = value; }
        }

        public IServerInformation Server
        {
            get { return innerBuilder.Server; }
            set { innerBuilder.Server = value; }
        }

        public IDictionary<string, object> Properties
        {
            get { return innerBuilder.Properties; }
            set { innerBuilder.Properties = value; }
        }
    }
}
