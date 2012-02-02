using System.ComponentModel.Composition;
using Glimpse.Core.Extensibility;
using Glimpse.NH.Plumbing.Injectors;

namespace Glimpse.NH.Service
{
    [GlimpseService]
    internal class NH : IGlimpseService
    {
        private readonly IGlimpseLogger _logger;
        private readonly INHibernateInfoProvider _nhibernateInfoProvider;
        private readonly IGlimpseProfileDbDriverFactory _profileDbDriverFactory;
        private readonly IGlimpseProfileDbDriverActivator _profileDbDriverActivator;

        [ImportingConstructor]
        public NH(IGlimpseFactory factory)
        {
            _logger = factory.CreateLogger();
            _nhibernateInfoProvider = new NHibernateInfoProvider();
            _profileDbDriverFactory = new GlimpseProfileDbDriverFactory();
            _profileDbDriverActivator = new GlimpseProfileDbDriverActivator();
        }

        public string Name
        {
            get { return "NH"; }
        }

        public void SetupInit()
        {
            _logger.Info("AdoPipelineInitiator for NH: Starting");

            var wrapDbDriver = new WrapDbDriver(_logger, _nhibernateInfoProvider, _profileDbDriverFactory, _profileDbDriverActivator);
            wrapDbDriver.Inject();

            _logger.Info("AdoPipelineInitiator for NH: Finished");
        }
    }
}