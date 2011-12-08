using System.ComponentModel.Composition;
using Glimpse.Core.Extensibility;
using Glimpse.NH.Plumbing.Injectors;

namespace Glimpse.NH.Service
{
    [GlimpseService]
    internal class NH : IGlimpseService
    {
        private IGlimpseLogger Logger { get; set; }
        
        [ImportingConstructor]
        public NH(IGlimpseFactory factory)
        {
            Logger = factory.CreateLogger();
        }

        public string Name
        {
            get { return "NH"; }
        }

        public void SetupInit()
        {
            Logger.Info("AdoPipelineInitiator for NH: Starting");

            var wrapDbDriver = new WrapDbDriver(Logger);
            wrapDbDriver.Inject();

            Logger.Info("AdoPipelineInitiator for NH: Finished");
        }
    }
}