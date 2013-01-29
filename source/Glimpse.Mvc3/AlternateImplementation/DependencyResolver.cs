using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class DependencyResolver : AlternateType<IDependencyResolver>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public DependencyResolver(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get 
            { 
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                {
                    new GetService(),
                    new GetServices(),
                }); 
            }
        }

        public class GetService : AlternateMethod
        {
            public GetService() : base(typeof(IDependencyResolver), "GetService")
            {
            }
            
            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                context.MessageBroker.Publish(new Message(context.TargetType, context.MethodInvocationTarget, (Type)context.Arguments[0], context.ReturnValue));
            }

            public class Message : MessageBase
            {
                public Message(Type executedType, MethodInfo executedMethod, Type serviceType, object resolvedObject)
                    : base(executedType, executedMethod)
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
                context.MessageBroker.Publish(new Message(context.TargetType, context.MethodInvocationTarget, (Type)context.Arguments[0], (IEnumerable<object>)context.ReturnValue));
            }

            public class Message : MessageBase
            {
                public Message(Type executedType, MethodInfo executedMethod, Type serviceType, IEnumerable<object> resolvedObjects)
                    : base(executedType, executedMethod)
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