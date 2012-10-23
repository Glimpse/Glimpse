using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class View
    {
        private View()
        {
        }

        public static IEnumerable<IAlternateImplementation<IView>> AllMethods(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            yield return new Render(messageBroker, timerStrategy, runtimePolicyStrategy);
        }

        public class Render : IAlternateImplementation<IView>
        {
            public Render(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
            {
                MessageBroker = messageBroker;
                TimerStrategy = timerStrategy;
                MethodToImplement = typeof(IView).GetMethod("Render");
                RuntimePolicyStrategy = runtimePolicyStrategy;
            }

            public IMessageBroker MessageBroker { get; set; }
            
            public Func<IExecutionTimer> TimerStrategy { get; set; }
            
            public MethodInfo MethodToImplement { get; private set; }
            
            public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                var input = new Arguments(context.Arguments);

                //// TODO: This is where we could use writer.Write calls to inject HTML comments

                var timer = TimerStrategy();
                var timing = timer.Time(context.Proceed);

                var mixin = context.Proxy as IViewCorrelation;

                MessageBroker.Publish(new Message(input, timing, context.TargetType, mixin));
                MessageBroker.Publish(new TimerResultMessage(timing, "Render View " + mixin.ViewName, "ASP.NET MVC")); // TODO: Clean this up
            }

            public class Arguments
            {
                public Arguments(object[] arguments)
                {
                    ViewContext = (ViewContext)arguments[0];
                    Writer = (TextWriter)arguments[1];
                }

                public ViewContext ViewContext { get; set; }
                
                public TextWriter Writer { get; set; }
            }

            public class Message
            {
                public Message(Arguments input, TimerResult timing, Type baseType, IViewCorrelation viewCorrelation)
                {
                    Input = input;
                    Timing = timing;
                    BaseType = baseType;
                    ViewCorrelation = viewCorrelation;
                }

                public Arguments Input { get; set; }
               
                public TimerResult Timing { get; set; }
                
                public Type BaseType { get; set; }
                
                public IViewCorrelation ViewCorrelation { get; set; }
            }
        }
    }
}