using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Routing;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.AspNet.AlternateImplementation
{
    public class RouteBase : Alternate<System.Web.Routing.RouteBase>
    {
        public RouteBase(IProxyFactory proxyFactory)
            : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<System.Web.Routing.RouteBase>> AllMethods()
        {
            yield return new GetRouteData<System.Web.Routing.RouteBase>();
        } 

        public class GetRouteData<T> : IAlternateImplementation<T>
            where T : System.Web.Routing.RouteBase
        {
            public GetRouteData()
            {
                MethodToImplement = typeof(T).GetMethod("GetRouteData", BindingFlags.Public | BindingFlags.Instance);
            }

            public MethodInfo MethodToImplement { get; set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timer;
                if (!context.TryProceedWithTimer(out timer))
                {
                    return;
                }

                var result = (RouteData)context.ReturnValue;

                context.MessageBroker.Publish(new Message((T)context.InvocationTarget, result));
            }
             
            public class Message
            {
                public Message(T route, RouteData routeData)
                {
                    RouteData = routeData; 
                    Route = route;
                }

                public RouteData RouteData { get; set; }

                public T Route { get; set; } 
            }
        }
    }
}
