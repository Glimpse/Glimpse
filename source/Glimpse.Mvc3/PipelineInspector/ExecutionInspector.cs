using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;

namespace Glimpse.Mvc.PipelineInspector
{
    public class ExecutionInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var proxyFactory = context.ProxyFactory;
            var logger = context.Logger;

            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            if (proxyFactory.IsProxyable(controllerFactory))
            {
                var alternateImplementations = ControllerFactory.AllMethods(context.RuntimePolicyStrategy, context.MessageBroker, context.ProxyFactory, context.TimerStrategy);

                var proxiedControllerFactory = proxyFactory.CreateProxy(controllerFactory, alternateImplementations);

                ControllerBuilder.Current.SetControllerFactory(proxiedControllerFactory);

                logger.Debug(Resources.ControllerFactorySetup, controllerFactory.GetType());
            }


        }

        public void Teardown(IPipelineInspectorContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}