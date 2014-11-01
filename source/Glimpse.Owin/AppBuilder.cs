using System;
using System.Collections.Generic;
using Glimpse.Owin.Middleware;
using Owin;

namespace Glimpse.Owin
{
    public class AppBuilder : IAppBuilder
    {
        private readonly IAppBuilder innerApp;
        private readonly MiddlewareManager manager;
        private readonly Guid builderId;

        public AppBuilder(IAppBuilder app)
        {
            innerApp = app;
            manager = MiddlewareManager.Instance;
            builderId = Guid.NewGuid();
            innerApp.Use<HeadMiddleware>(Properties); // This is the earliest we can add middleware
        }

        public IDictionary<string, object> Properties
        {
            get { return innerApp.Properties; }
        }

        public IAppBuilder Use(object middleware, params object[] args)
        {
            var middlewareType = middleware is Type ? middleware as Type : middleware.GetType();
            manager.Register(builderId, middlewareType);

            innerApp.Use<GlimpseMiddleware>(middlewareType, builderId);
            innerApp.Use(middleware, args);
            
            return this;
        }

        public object Build(Type returnType)
        {
            return innerApp.Build(returnType);
        }

        public IAppBuilder New()
        {
            return new AppBuilder(innerApp.New());
        }
    }
}
