using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc3.PipelineInspector
{
    public class ControllerFactory:IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();
                
                var proxyFactory = context.ProxyFactory;
                if (proxyFactory.IsProxyable(controllerFactory))
                {
                    var logger = context.Logger;

                    var alternateImplementations = AlternateImplementation.ControllerFactory.AllMethods(context.RuntimePolicyStrategy, context.MessageBroker);

                    var proxy = proxyFactory.CreateProxy(controllerFactory, alternateImplementations);

                    ControllerBuilder.Current.SetControllerFactory(proxy);

                    logger.Debug(Resources.ControllerFactorySetup, controllerFactory.GetType());
                }
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}