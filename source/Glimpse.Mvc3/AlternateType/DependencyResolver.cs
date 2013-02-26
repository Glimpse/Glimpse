using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateType
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
            
            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var message = new Message((Type)context.Arguments[0], context.ReturnValue)
                    .AsSourceMessage(context.TargetType, context.MethodInvocationTarget);

                context.MessageBroker.Publish(message);
            }

            public class Message : MessageBase, ISourceMessage
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

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }

        public class GetServices : AlternateMethod
        {
            public GetServices() : base(typeof(IDependencyResolver), "GetServices")
            {
            }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var message = new Message((Type)context.Arguments[0], (IEnumerable<object>)context.ReturnValue)
                    .AsSourceMessage(context.TargetType, context.MethodInvocationTarget);

                context.MessageBroker.Publish(message);
            }

            public class Message : MessageBase, ISourceMessage
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

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }
    }
}