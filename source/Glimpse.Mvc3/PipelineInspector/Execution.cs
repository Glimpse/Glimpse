using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.PipelineInspector
{
    public class Execution : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var proxyFactory = context.ProxyFactory;
            var logger = context.Logger;

            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            if (proxyFactory.IsProxyable(controllerFactory))
            {
                var alternateImplementations = AlternateImplementation.ControllerFactory.AllMethods(context.RuntimePolicyStrategy, context.MessageBroker);

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