using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility; 
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
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

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                var arguments = new Arguments(IsPartial, context.Arguments);
                var id = Guid.NewGuid();

                var output = context.ReturnValue as ViewEngineResult; 
                output = ProxyOutput(output, context, arguments.ViewName, IsPartial, id);

                context.MessageBroker.Publish(new Message(arguments, timerResult, context.TargetType, context.MethodInvocationTarget, output, context.TargetType, IsPartial, id));
            }

            private ViewEngineResult ProxyOutput(ViewEngineResult viewEngineResult, IAlternateImplementationContext context, string viewName, bool isPartial, Guid id)
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

            public class Message : ActionMessage
            {
                public Message(Arguments arguments, TimerResult timerResult, Type executedType, MethodInfo method, ViewEngineResult output, Type baseType, bool isPartial, Guid id)
                    : base(timerResult, GetControllerName(arguments.ControllerContext), GetActionName(arguments.ControllerContext), GetIsChildAction(arguments.ControllerContext), executedType, method)
                {
                    ViewName = arguments.ViewName;
                    MasterName = arguments.MasterName;
                    UseCache = arguments.UseCache;
                    SearchedLocations = output.SearchedLocations; 
                    IsPartial = isPartial;
                    BaseType = baseType;
                    Id = id;
                    IsFound = output.View != null;
                }

                public string ViewName { get; protected set; }

                public string MasterName { get; protected set; }

                public bool UseCache { get; protected set; }

                public IEnumerable<string> SearchedLocations { get; protected set; }

                public Type BaseType { get; protected set; }

                public bool IsPartial { get; protected set; }

                public bool IsFound { get; protected set; }
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
        }
    }
}