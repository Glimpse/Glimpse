using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.AlternateImplementation
{
    public abstract class DependencyResolver
    {
        public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
        public IMessageBroker MessageBroker { get; set; }

        protected DependencyResolver(Func<RuntimePolicy> runtimePolicyStrategy, IMessageBroker messageBroker)
        {
            RuntimePolicyStrategy = runtimePolicyStrategy;
            MessageBroker = messageBroker;
        }

        public static IEnumerable<IAlternateImplementation<IDependencyResolver>> AllMethods(Func<RuntimePolicy> runtimePolicyStrategy, IMessageBroker messageBroker)
        {
            yield return new GetService(runtimePolicyStrategy, messageBroker);
        }

        //TODO Implement GetServices

        public class GetService : DependencyResolver, IAlternateImplementation<IDependencyResolver>
        {
            public GetService(Func<RuntimePolicy> runtimePolicyStrategy, IMessageBroker messageBroker):base(runtimePolicyStrategy, messageBroker){}

            public MethodInfo MethodToImplement { get { return typeof (IDependencyResolver).GetMethod("GetService"); }}
            
            public void NewImplementation(IAlternateImplementationContext context)
            {
                context.Proceed();

                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                    return;

                var message = new Message((Type) context.Arguments[0], context.ReturnValue);
                MessageBroker.Publish(message);

                //TODO: Handle IController's with IActionInvoker
            }

            public class Message
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
    }
}