using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class View : AlternateType<IView>
    {
        private List<IAlternateMethod> allMethods;

        public View(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new Render()
                    });
            }
        }

        public class Render : AlternateMethod
        {
            public Render() : base(typeof(IView), "Render")
            {
            }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timing)
            { 
                var mixin = context.Proxy as IViewCorrelationMixin;

                context.MessageBroker.Publish(new Message(new Arguments(context.Arguments), context.InvocationTarget.GetType(), context.MethodInvocationTarget, timing, context.TargetType, mixin));
            }

            public class Arguments
            {
                public Arguments(params object[] arguments)
                {
                    ViewContext = (ViewContext)arguments[0];
                    Writer = (TextWriter)arguments[1];
                }

                public ViewContext ViewContext { get; set; }
                
                public TextWriter Writer { get; set; }
            }

            public class Message : ActionMessage, ITimelineMessage
            {
                public Message(Arguments arguments, Type executedType, MethodInfo method, TimerResult timerResult, Type baseType, IViewCorrelationMixin viewCorrelation)
                    : base(timerResult, GetControllerName(arguments.ViewContext.Controller), GetActionName(arguments.ViewContext.Controller), GetIsChildAction(arguments.ViewContext.Controller), executedType, method)
                {
                    EventName = string.Format("Render:View - {0}:{1}", ControllerName, ActionName);
                    EventCategory = "View"; 
                    Input = arguments; 
                    BaseType = baseType;
                    ViewCorrelation = viewCorrelation;
                    ViewData = arguments.ViewContext.ViewData;
                    TempData = arguments.ViewContext.TempData;
                    ModelMetadata = arguments.ViewContext.ViewData.ModelMetadata;
                    ModelStateIsValid = arguments.ViewContext.ViewData.ModelState.IsValid;
                    ViewDataModelType = arguments.ViewContext.ViewData.Model.GetTypeOrNull(); 
                }

                public Type ViewDataModelType { get; private set; }

                public Arguments Input { get; private set; }

                public bool ModelStateIsValid { get; private set; }

                public IDictionary<string, object> TempData { get; private set; }

                public IDictionary<string, object> ViewData { get; private set; }

                public ModelMetadata ModelMetadata { get; private set; }

                public Type BaseType { get; private set; }

                public IViewCorrelationMixin ViewCorrelation { get; private set; }
            } 
        }
    }
}