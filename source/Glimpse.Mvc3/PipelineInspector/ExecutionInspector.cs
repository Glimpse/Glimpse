using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;

namespace Glimpse.Mvc.PipelineInspector
{
    public class ExecutionInspector : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {
            var logger = context.Logger;

            var originalControllerFactory = ControllerBuilder.Current.GetControllerFactory();
            var alternateImplementation = new ControllerFactory(context.ProxyFactory);
            IControllerFactory newControllerFactory;

            if (alternateImplementation.TryCreate(originalControllerFactory, out newControllerFactory))
            {
                ControllerBuilder.Current.SetControllerFactory(newControllerFactory);

                logger.Debug(Resources.ControllerFactorySetup, originalControllerFactory.GetType());
            }
        }
    }
}