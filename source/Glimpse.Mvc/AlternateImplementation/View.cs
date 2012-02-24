using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class View
    {
        protected internal IMessageBroker MessageBroker { get; set; }
        protected internal Func<IExecutionTimer> TimerStrategy { get; set; }

        private View(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            MessageBroker = messageBroker;
            TimerStrategy = timerStrategy;
        }

        public static IEnumerable<IAlternateImplementation<IView>> All(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            yield return new Render(messageBroker, timerStrategy);
        }

        public class Render : View, IAlternateImplementation<IView>
        {
            public Render(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy) : base(messageBroker, timerStrategy) { }

            private readonly MethodInfo methodToImplement = typeof (IView).GetMethod("Render");
            public MethodInfo MethodToImplement
            {
                get { return methodToImplement; }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                var arguments = context.Arguments;
                var viewContext = (ViewContext) arguments[0];
                //var writer = (TextWriter) arguments[1];
                //TODO: This is where we could use writer.Write calls to inject HTML comments

                var timer = TimerStrategy();

                var timerResult = timer.Time(context.Proceed);

                MessageBroker.Publish(new ViewRenderCall(timerResult, viewContext, context.Proxy as IViewMixin));
            }
        }
    }
}