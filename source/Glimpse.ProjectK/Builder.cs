using System;
using System.Collections.Generic;
using Glimpse.ProjectK.Middleware;
using Microsoft.AspNet.Builder;

namespace Glimpse.ProjectK
{
    public class Builder : IBuilder
    {
        private readonly IBuilder innerBuilder;
        private readonly static IDictionary<string, object> ApplicationStore = new Dictionary<string, object>();
        private readonly Guid builderId;
        private readonly MiddlewareManager manager;

        public Builder(IBuilder app)
        {
            innerBuilder = app;
            manager = MiddlewareManager.Instance;
            builderId = Guid.NewGuid();
            innerBuilder.Use(next => new GlimpseMiddleware(next, ApplicationStore).Invoke); // This is the earliest we can add middleware
        }
         
        public IBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            manager.Register(builderId, middleware);

            innerBuilder.Use(next => new HeadMiddleware(next, middleware, builderId).Invoke);

            innerBuilder.Use(middleware);
            return this;
        }

        public IBuilder New()
        {
            return new Builder(innerBuilder.New());
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
    }
}
