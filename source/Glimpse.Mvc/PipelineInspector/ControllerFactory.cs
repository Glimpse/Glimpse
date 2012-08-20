using System.Web.Mvc;
using Glimpse.Core.Extensibility;

namespace Glimpse.Mvc.PipelineInspector
{
    public class ControllerFactory:IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var controllerFactory = ControllerBuilder.Current.GetControllerFactory();

            if (controllerFactory != null) //If null, no need to proxy
            {
                var logger = context.Logger;
                var proxyFactory = context.ProxyFactory;

                if (proxyFactory.IsProxyable(controllerFactory))
                {
                    //proxyFactory.CreateProxy(controllerFactory, )
                }

                //ControllerBuilder.Current.SetControllerFactory(controllerBuilder.Proxy(Logger));
            }
        }

        public void Teardown(IPipelineInspectorContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}