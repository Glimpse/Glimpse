using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Message;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateType
{
    public class ViewEngine : AlternateType<IViewEngine>
    {
        private List<IAlternateMethod> allMethods;

        public ViewEngine(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                if (allMethods == null)
                {
                    var alternateView = new View(ProxyFactory);
                    allMethods = new List<IAlternateMethod>
                        {
                            new FindViews(false, alternateView),
                            new FindViews(true, alternateView)
                        };
                }

                return allMethods;
            }
        }

        // This class is the alternate implementation for both .FindView() AND .FindPartialView()
        public class FindViews : AlternateMethod
        {
            public FindViews(bool isPartial, AlternateType<IView> alternateView) : base(typeof(IViewEngine), isPartial ? "FindPartialView" : "FindView")
            {
                AlternateView = alternateView;
                IsPartial = isPartial;
            }

            public AlternateType<IView> AlternateView { get; set; }

            public bool IsPartial { get; set; }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var args = new Arguments(IsPartial, context.Arguments);
                var id = Guid.NewGuid();

                var output = context.ReturnValue as ViewEngineResult;
                output = ProxyOutput(output, context, args.ViewName, IsPartial, id);

                string displayModeId = null;
                Type displayModeType = null;

#if MVC4
                var displayMode = args.ControllerContext.DisplayMode;
                if (displayMode != null)
                {
                    displayModeId = displayMode.DisplayModeId;
                    displayModeType = displayMode.GetType();
                }
#endif

                var message = new Message(id, args.ViewName, args.MasterName, args.UseCache, output.SearchedLocations, context.TargetType, IsPartial, output.View != null, displayModeId, displayModeType)
                    .AsActionMessage(args.ControllerContext)
                    .AsChildActionMessage(args.ControllerContext)
                    .AsSourceMessage(context.InvocationTarget.GetType(), context.MethodInvocationTarget)
                    .AsTimedMessage(timerResult);

                context.MessageBroker.Publish(message);  
            }

            private ViewEngineResult ProxyOutput(ViewEngineResult viewEngineResult, IAlternateMethodContext context, string viewName, bool isPartial, Guid id)
            {
                if (viewEngineResult.View != null)
                {
                    var originalView = viewEngineResult.View;

                    IView newView;
                    if (AlternateView.TryCreate(originalView, out newView, new[] { new ViewCorrelationMixin(viewName, isPartial, id) }))
                    {
                        context.Logger.Info(Resources.FindViewsProxyOutputReplacedIView, originalView.GetType(), viewName);

                        var result = new ViewEngineResult(newView, viewEngineResult.ViewEngine);
                        context.ReturnValue = result;
                        return result;
                    }
                }

                return viewEngineResult;
            }

            public class Arguments
            {
                public Arguments(bool isPartial, params object[] arguments)
                {
                    ControllerContext = (ControllerContext)arguments[0];
                    ViewName = (string)arguments[1];
                    UseCache = isPartial ? (bool)arguments[2] : (bool)arguments[3];
                    MasterName = isPartial ? string.Empty : (string)arguments[2];
                }

                public ControllerContext ControllerContext { get; set; }

                public string ViewName { get; set; }

                public string MasterName { get; set; }

                public bool UseCache { get; set; }
            }

            public class Message : ISourceMessage, IChildActionMessage, ITimedMessage
            {
                public Message(Guid id, string viewName, string masterName, bool useCache, IEnumerable<string> searchedLocations, Type baseType, bool isPartial, bool isFound, string displayModeId, Type displayModeType)
                {
                    Id = id;
                    ViewName = viewName;
                    MasterName = masterName;
                    UseCache = useCache;
                    SearchedLocations = searchedLocations;
                    BaseType = baseType;
                    IsPartial = isPartial;
                    IsFound = isFound;
                    DisplayModeId = displayModeId;
                    DisplayModeType = displayModeType;
                }

                public Guid Id { get; private set; }
                
                public string ControllerName { get; set; }
                
                public string ActionName { get; set; }
                
                public bool IsChildAction { get; set; }
                
                public TimeSpan Offset { get; set; }
                
                public TimeSpan Duration { get; set; }
                
                public DateTime StartTime { get; set; }
                
                public string EventName { get; set; }

                public string DisplayModeId { get; set; }

                public Type DisplayModeType { get; set; }
                
                public TimelineCategoryItem EventCategory { get; set; }
                
                public string EventSubText { get; set; }
                
                public string ViewName { get; protected set; } 
                
                public string MasterName { get; protected set; } 
                
                public bool UseCache { get; protected set; } 
                
                public IEnumerable<string> SearchedLocations { get; protected set; } 
                
                public Type BaseType { get; protected set; } 
                
                public bool IsPartial { get; protected set; } 
                
                public bool IsFound { get; protected set; }

                public Type ExecutedType { get; set; }

                public MethodInfo ExecutedMethod { get; set; }
            }
        }
    }
}