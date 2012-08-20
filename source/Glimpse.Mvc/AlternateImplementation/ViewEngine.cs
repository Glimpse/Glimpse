using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core;
using Glimpse.Mvc.Message;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ViewEngine
    {
        private ViewEngine()
        {
        }

        public static IEnumerable<IAlternateImplementation<IViewEngine>> AllMethods(IMessageBroker messageBroker, IProxyFactory proxyFactory, ILogger logger, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy)
        {
            yield return new FindViews(messageBroker, proxyFactory, logger, timerStrategy, runtimePolicyStrategy, false);
            yield return new FindViews(messageBroker, proxyFactory, logger, timerStrategy, runtimePolicyStrategy, true);
        }

        //This class is the alternate implementation for both .FindView() AND .FindPartialView()
        public class FindViews : IAlternateImplementation<IViewEngine>
        {
            public IMessageBroker MessageBroker { get; set; }
            public IProxyFactory ProxyFactory { get; set; }
            public ILogger Logger { get; set; }
            public Func<IExecutionTimer> TimerStrategy { get; set; }
            public Func<RuntimePolicy> RuntimePolicyStrategy { get; set; }
            public bool IsPartial { get; set; }
            public MethodInfo MethodToImplement { get; private set; }

            public FindViews(IMessageBroker messageBroker, IProxyFactory proxyFactory, ILogger logger, Func<IExecutionTimer> timerStrategy, Func<RuntimePolicy> runtimePolicyStrategy, bool isPartial)
            {
                MessageBroker = messageBroker;
                ProxyFactory = proxyFactory;
                Logger = logger;
                TimerStrategy = timerStrategy;
                IsPartial = isPartial;
                RuntimePolicyStrategy = runtimePolicyStrategy;
                MethodToImplement = typeof (IViewEngine).GetMethod(IsPartial ? "FindPartialView" : "FindView");
            }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                if (RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                var timer = TimerStrategy();

                var timing = timer.Time(context.Proceed);

                var input = new Arguments(context.Arguments, IsPartial);

                var id = Guid.NewGuid();

                var output = context.ReturnValue as ViewEngineResult;

                output = ProxyOutput(output, context, input.ViewName, IsPartial, id);

                MessageBroker.Publish(new Message(input, output, timing, context.TargetType, IsPartial, id));
                MessageBroker.Publish(new TimerResultMessage(timing, "Find View " + input.ViewName, "ASP.NET MVC"));//TODO: Clean this up
            }

            private ViewEngineResult ProxyOutput(ViewEngineResult viewEngineResult, IAlternateImplementationContext context, string viewName, bool isPartial, Guid id)
            {
                if (viewEngineResult.View != null)
                {
                    var originalView = viewEngineResult.View;

                    if (ProxyFactory.IsProxyable(originalView))
                    {
                        var newView = ProxyFactory.CreateProxy(originalView,
                                                               AlternateImplementation.View.AllMethods(MessageBroker, TimerStrategy, RuntimePolicyStrategy),
                                                               new View.Render.Mixin(viewName, isPartial, id));

                        Logger.Info(Resources.FindViewsProxyOutputReplacedIView, originalView.GetType(), viewName);

                        var result = new ViewEngineResult(newView, viewEngineResult.ViewEngine);
                        context.ReturnValue = result;
                        return result;
                    }
                }
                return viewEngineResult;
            }

            public class Message
            {
                public Message(Arguments input, ViewEngineResult output, TimerResult timing, Type baseType,
                               bool isPartial, Guid id)
                {
                    if (input == null) throw new ArgumentNullException("input");
                    if (output == null) throw new ArgumentNullException("output");
                    if (timing == null) throw new ArgumentNullException("timing");

                    Input = input;
                    Output = output;
                    Timing = timing;

                    IsPartial = isPartial;
                    BaseType = baseType;
                    Id = id;
                }

                public Arguments Input { get; set; }
                public ViewEngineResult Output { get; set; }
                public TimerResult Timing { get; set; }
                public Type BaseType { get; set; }

                public bool IsPartial { get; set; }
                public Guid Id { get; set; }

                public bool IsFound
                {
                    get { return Output.View != null; }
                }
            }

            public class Arguments
            {
                public Arguments(object[] arguments, bool isPartial)
                {
                    ControllerContext = (ControllerContext) arguments[0];
                    ViewName = (string) arguments[1];
                    UseCache = isPartial ? (bool) arguments[2] : (bool) arguments[3];
                    MasterName = isPartial ? string.Empty : (string) arguments[2];
                }

                public ControllerContext ControllerContext { get; set; }
                public string ViewName { get; set; }
                public string MasterName { get; set; }
                public bool UseCache { get; set; }
            }
        }
    }
}