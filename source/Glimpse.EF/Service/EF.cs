using Glimpse.Ado.Plumbing;
using Glimpse.Core.Extensibility;
using Glimpse.EF.Plumbing.Injectors;

namespace Glimpse.EF.Service
{
    public class EF : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            var logger = context.Logger;

            logger.Info("AdoPipelineInitiator for EF: Starting");

            ProviderStats.MessageBroker = context.MessageBroker;

            var wrapDbProviderFactories = new WrapDbProviderFactories(logger);
            wrapDbProviderFactories.Inject();

            var wrapDbConnectionFactories = new WrapDbConnectionFactories(logger);
            wrapDbConnectionFactories.Inject();

            var wrapCachedMetadata = new WrapCachedMetadata(logger);
            wrapCachedMetadata.Inject();

            logger.Info("AdoPipelineInitiator for EF: Finished");
        }
    }
}