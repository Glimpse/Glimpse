using System.ComponentModel.Composition;
using Glimpse.Core.Extensibility;
using Glimpse.EF.Plumbing.Injectors;

namespace Glimpse.EF.Service
{
    [GlimpseService]
    internal class EF : IGlimpseService
    {
        private IGlimpseLogger Logger { get; set; }
        
        [ImportingConstructor]
        public EF(IGlimpseFactory factory)
        {
            Logger = factory.CreateLogger();
        }

        public string Name
        {
            get { return "EF"; }
        }

        public void SetupInit()
        {
            Logger.Info("AdoPipelineInitiator for EF: Starting");

            var wrapDbProviderFactories = new WrapDbProviderFactories(Logger);
            wrapDbProviderFactories.Inject();

            var wrapDbConnectionFactories = new WrapDbConnectionFactories(Logger);
            wrapDbConnectionFactories.Inject();

            var wrapCachedMetadata = new WrapCachedMetadata(Logger);
            wrapCachedMetadata.Inject();

            Logger.Info("AdoPipelineInitiator for EF: Finished");
        }
    }
}