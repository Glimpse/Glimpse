using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ViewEngine
    {
        protected internal Func<IExecutionTimer> TimerStrategy { get; set; }
        protected internal IMessageBroker MessageBroker { get; set; }
        protected internal IProxyFactory ProxyFactory { get; set; }

        protected ViewEngine(IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy)
        {
            TimerStrategy = timerStrategy;
            MessageBroker = messageBroker;
            ProxyFactory = proxyFactory;
        }

        public static IEnumerable<IAlternateImplementation<IViewEngine>> All(IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy)
        {
            yield return new FindViews(messageBroker, proxyFactory, timerStrategy, false);
            yield return new FindViews(messageBroker, proxyFactory, timerStrategy, true);
        }

        public class FindViews : ViewEngine, IAlternateImplementation<IViewEngine>
        {
            public bool IsPartial { get; set; }

            public FindViews(IMessageBroker messageBroker, IProxyFactory proxyFactory, Func<IExecutionTimer> timerStrategy, bool isPartial) : base(messageBroker, proxyFactory, timerStrategy)
            {
                IsPartial = isPartial;

                methodToImplement = typeof (IViewEngine).GetMethod(IsPartial ? "FindPartialView" : "FindView");
            }

            private readonly MethodInfo methodToImplement = typeof (IViewEngine).GetMethod("FindView");

            public MethodInfo MethodToImplement
            {
                get { return methodToImplement; }
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                var timer = TimerStrategy();

                var timerResult = timer.Time(context.Proceed);

                var arguments = context.Arguments;
                var viewName = (string)arguments[1];

                string masterName;
                bool useCache;

                if (IsPartial) //slightly different parameters between FindPartialView & FindView
                {
                    masterName = "";
                    useCache = (bool)arguments[2];
                }
                else
                {
                    masterName = (string)arguments[2];
                    useCache = (bool)arguments[3];
                }

                var viewEngineResult = context.ReturnValue as ViewEngineResult;
                IView newView = ConfigureView(viewEngineResult, context, viewName, IsPartial);

                var message = new ViewEngineFindCall(timerResult,
                                                 viewEngineResult,
                                                 newView,
                                                 IsPartial,//isPartial
                                                 viewName,
                                                 masterName,
                                                 useCache,
                                                 context.TargetType);//viewEngineType

                MessageBroker.Publish(message);
            }

            private IView ConfigureView(ViewEngineResult viewEngineResult, IAlternateImplementationContext context, string viewName, bool isPartial)
            {
                if (viewEngineResult != null && viewEngineResult.View != null)
                {
                    var originalView = viewEngineResult.View;

                    if (ProxyFactory.IsProxyable(originalView))
                    {
                        var newView = ProxyFactory.CreateProxy(originalView,
                                                           AlternateImplementation.View.All(MessageBroker, TimerStrategy),
                                                           new ViewMixin{ViewName = viewName, IsPartial = isPartial});

                        context.ReturnValue = new ViewEngineResult(newView, viewEngineResult.ViewEngine);
                        return newView;
                    }
                }
                return null;
            }
        }
    }
}