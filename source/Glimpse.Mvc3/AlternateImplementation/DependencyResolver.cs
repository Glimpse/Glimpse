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

        public class GetService : IAlternateMethod
        {
            public GetService()
            {
                MethodToImplement = typeof(IDependencyResolver).GetMethod("GetService");
            }

            public MethodInfo MethodToImplement { get; private set; }
            
            public void NewImplementation(IAlternateImplementationContext context)
            {
                context.Proceed();

                if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    return;
                }

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

        public class GetServices : IAlternateMethod
        {
            public GetServices()
            {
                MethodToImplement = typeof(IDependencyResolver).GetMethod("GetServices");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                context.Proceed();

                if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    return;
                }

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