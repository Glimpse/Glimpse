using Glimpse.Ado.Messages;
using Glimpse.Core.Extensibility;
using Glimpse.EF.Plumbing.Injectors;

namespace Glimpse.EF.Service
{
    internal class EF : IPipelineInspector
    {
        public void Setup(IPipelineInspectorContext context)
        {           
            var logger = context.Logger;
            logger.Info("AdoPipelineInitiator for EF: Starting");

            var wrapDbProviderFactories = new WrapDbProviderFactories(context);
            wrapDbProviderFactories.Inject();

            var wrapDbConnectionFactories = new WrapDbConnectionFactories();
            wrapDbConnectionFactories.Inject();

            var wrapCachedMetadata = new WrapCachedMetadata();
            wrapCachedMetadata.Inject();

            logger.Info("AdoPipelineInitiator for EF: Finished");             
        }
    }
}