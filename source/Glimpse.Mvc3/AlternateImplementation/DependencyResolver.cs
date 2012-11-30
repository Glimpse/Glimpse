using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class DependencyResolver : Alternate<IDependencyResolver>
    {
        public DependencyResolver(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods()
        {
            yield return new GetService();
            yield return new GetServices();
        }

        public class GetService : AlternateMethod
        {
            public GetService() : base(typeof(IDependencyResolver), "GetService")
            {
            }
            
            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                var resolvedObject = context.ReturnValue;
                context.MessageBroker.Publish(new Message((Type)context.Arguments[0], resolvedObject));
            }

            public class Message : MessageBase
            {
                public Message(Type serviceType, object resolvedObject)
                {
                    ServiceType = serviceType;
                    
                    if (resolvedObject != null)
                    {
                        IsResolved = true;
                        ResolvedType = resolvedObject.GetType();
                    }
                }

                public Type ServiceType { get; set; }
                
                public Type ResolvedType { get; set; }
                
                public bool IsResolved { get; set; }
            }
        }

        public class GetServices : AlternateMethod
        {
            public GetServices() : base(typeof(IDependencyResolver), "GetServices")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(
                    new Message((Type)context.Arguments[0], (IEnumerable<object>)context.ReturnValue));
            }

            public class Message : MessageBase
            {
                public Message(Type serviceType, IEnumerable<object> resolvedObjects)
                {
                    ServiceType = serviceType;
                    
                    if (resolvedObjects != null && resolvedObjects.Any())
                    {
                        IsResolved = true;
                        ResolvedTypes = resolvedObjects.Select(obj => obj.GetType());
                    }
                }

                public Type ServiceType { get; set; }
                
                public IEnumerable<Type> ResolvedTypes { get; set; }
                
                public bool IsResolved { get; set; }
            }
        }
    }
}