using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Async;
using Glimpse.Core;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class AsyncActionInvoker : AlternateType<AsyncControllerActionInvoker>
    {
        public AsyncActionInvoker(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods()
        {
            yield return new BeginInvokeActionMethod();
            yield return new EndInvokeActionMethod();
            yield return new ActionInvoker.InvokeActionResult<AsyncControllerActionInvoker>();
            yield return new ActionInvoker.GetFilters<AsyncControllerActionInvoker>(new ActionFilter(ProxyFactory), new ResultFilter(ProxyFactory), new AuthorizationFilter(ProxyFactory), new ExceptionFilter(ProxyFactory));
        }

        public class BeginInvokeActionMethod : IAlternateMethod
        {
            public BeginInvokeActionMethod()
            {
                MethodToImplement = typeof(AsyncControllerActionInvoker).GetMethod("BeginInvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                // BeginInvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters, AsyncCallback callback, object state)
                if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                var state = (IActionInvokerStateMixin)context.Proxy;
                var timer = context.TimerStrategy();
                state.Arguments = new ActionInvoker.InvokeActionMethod.Arguments(context.Arguments);
                state.Offset = timer.Start();
                context.Proceed();
            }
        }

        public class EndInvokeActionMethod : IAlternateMethod
        {
            public EndInvokeActionMethod()
            {
                MethodToImplement = typeof(AsyncControllerActionInvoker).GetMethod("EndInvokeActionMethod", BindingFlags.Instance | BindingFlags.NonPublic);
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                if (context.RuntimePolicyStrategy() == RuntimePolicy.Off)
                {
                    context.Proceed();
                    return;
                }

                context.Proceed();
                var state = (IActionInvokerStateMixin)context.Proxy;
                var timer = context.TimerStrategy();
                var timerResult = timer.Stop(state.Offset);

                context.MessageBroker.Publish(new ActionInvoker.InvokeActionMethod.Message(
                    state.Arguments, 
                    (ActionResult)context.ReturnValue, 
                    context.MethodInvocationTarget, 
                    timerResult));
            }
        }
    }
}