using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class View : Alternate<IView>
    {
        public View(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<IView>> AllMethods()
        {
            yield return new Render();
        }

        public class Render : IAlternateImplementation<IView>
        {
            public Render()
            {
                MethodToImplement = typeof(IView).GetMethod("Render");
            }

            public MethodInfo MethodToImplement { get; private set; }
            
            public void NewImplementation(IAlternateImplementationContext context)
            {
                if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                var input = new Arguments(context.Arguments);

                //// TODO: This is where we could use writer.Write calls to inject HTML comments

                var timer = context.TimerStrategy();
                var timing = timer.Time(context.Proceed);

                var mixin = context.Proxy as IViewCorrelationMixin;

                context.MessageBroker.PublishMany(
                    new Message(input, timing, context.TargetType, mixin),
                    new TimerResultMessage(timing, "Render View " + mixin.ViewName, "ASP.NET MVC"));
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

            public class Message : MessageBase
            {
                public Message(Arguments input, TimerResult timing, Type baseType, IViewCorrelationMixin viewCorrelation)
                {
                    Input = input;
                    Timing = timing;
                    BaseType = baseType;
                    ViewCorrelation = viewCorrelation;
                }

                public Arguments Input { get; set; }
               
                public TimerResult Timing { get; set; }
                
                public Type BaseType { get; set; }
                
                public IViewCorrelationMixin ViewCorrelation { get; set; }
            }
        }
    }
}