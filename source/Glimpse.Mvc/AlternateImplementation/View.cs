using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class View
    {
        private View(){}

        public static IEnumerable<IAlternateImplementation<IView>> AllMethods(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
        {
            yield return new Render(messageBroker, timerStrategy);
        }

        public class Render : IAlternateImplementation<IView>
        {
            public IMessageBroker MessageBroker { get; set; }
            public Func<IExecutionTimer> TimerStrategy { get; set; }
            public MethodInfo MethodToImplement { get; private set; }

            public Render(IMessageBroker messageBroker, Func<IExecutionTimer> timerStrategy)
            {
                MessageBroker = messageBroker;
                TimerStrategy = timerStrategy;
                MethodToImplement = typeof (IView).GetMethod("Render");
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                var input = new Arguments(context.Arguments);

                //TODO: This is where we could use writer.Write calls to inject HTML comments

                var timer = TimerStrategy();
                var timing = timer.Time(context.Proceed);

                var mixin = context.Proxy as IMixin;

                MessageBroker.Publish(new Message(input, timing, context.TargetType, mixin));
                MessageBroker.Publish(new TimerResultMessage(timing, "Render View " + mixin.ViewName, "ASP.NET MVC"));//TODO: Clean this up
            }

            public class Arguments
            {
                public Arguments(object [] arguments)
                {
                    ViewContext = (ViewContext)arguments[0];
                    Writer = (TextWriter) arguments[1];
                }

                public ViewContext ViewContext { get; set; }
                public TextWriter Writer { get; set; }
            }

            public class Message
            {
                public Message(Arguments input, TimerResult timing, Type baseType, IMixin mixin)
                {
                    Input = input;
                    Timing = timing;
                    BaseType = baseType;
                    Mixin = mixin;
                }

                public Arguments Input { get; set; }
                public TimerResult Timing { get; set; }
                public Type BaseType { get; set; }
                public IMixin Mixin { get; set; }
            }

            public interface IMixin
            {
                string ViewName { get; }
                bool IsPartial { get; }
                Guid ViewEngineFindCallId { get; }
            }

            public class Mixin : IMixin
            {
                public Mixin(string viewName, bool isPartial, Guid viewEngineFindCallId)
                {
                    ViewName = viewName;
                    IsPartial = isPartial;
                    ViewEngineFindCallId = viewEngineFindCallId;
                }

                public string ViewName { get; set; }
                public bool IsPartial { get; set; }
                public Guid ViewEngineFindCallId { get; set; }
            }
        }
    }
}