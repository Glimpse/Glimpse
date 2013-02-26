using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Message;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateType
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

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timing)
            {
                var mixin = context.Proxy as IViewCorrelationMixin;
                var args = new Arguments(context.Arguments);
                var message = new Message(args.ViewContext.ViewData.Model.GetTypeOrNull(), args.ViewContext.ViewData.ModelState.IsValid, args.ViewContext.TempData, args.ViewContext.ViewData, args.ViewContext.ViewData.ModelMetadata, context.TargetType, mixin)
                    .AsActionMessage(args.ViewContext.Controller)
                    .AsTimedMessage(timing)
                    .AsMvcTimelineMessage(Glimpse.Mvc.Message.Timeline.View);

                context.MessageBroker.Publish(message);
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

            public class Message : MessageBase, IActionMessage, ITimelineMessage
            {
                public Message(Type viewDataModelType, bool modelStateIsValid, IDictionary<string, object> tempData, IDictionary<string, object> viewData, ModelMetadata modelMetadata, Type baseType, IViewCorrelationMixin viewCorrelation)
                {
                    ViewDataModelType = viewDataModelType;
                    ModelStateIsValid = modelStateIsValid;
                    TempData = tempData;
                    ViewData = viewData;
                    ModelMetadata = modelMetadata;
                    BaseType = baseType;
                    ViewCorrelation = viewCorrelation;
                }

                public string ControllerName { get; set; }
                
                public string ActionName { get; set; }
                
                public TimeSpan Offset { get; set; }
                
                public TimeSpan Duration { get; set; }
                
                public DateTime StartTime { get; set; }
                
                public string EventName { get; set; }
                
                public TimelineCategory EventCategory { get; set; }
                
                public string EventSubText { get; set; }
                
                public Type ViewDataModelType { get; private set; }  
                
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