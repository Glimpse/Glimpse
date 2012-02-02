using System;
using System.Linq;
using Glimpse.Core.Extensibility;

namespace Glimpse.NH.Plumbing.Injectors
{
    public class WrapDbDriver
    {
        private readonly IGlimpseLogger _logger;
        private readonly INHibernateInfoProvider _nhibernateInfoProvider;
        private readonly IGlimpseProfileDbDriverFactory _profileDbDriverFactory;
        private readonly IGlimpseProfileDbDriverActivator _profileDbDriverActivator;

        public WrapDbDriver(IGlimpseLogger logger, INHibernateInfoProvider nhibernateInfoProvider, IGlimpseProfileDbDriverFactory profileDbDriverFactory, IGlimpseProfileDbDriverActivator profileDbDriverActivator)
        {
            if (logger == null)
                throw new ArgumentNullException("logger");

            if (nhibernateInfoProvider == null)
                throw new ArgumentNullException("nhibernateInfoProvider");

            if (profileDbDriverFactory == null) 
                throw new ArgumentNullException("profileDbDriverFactory");

            if (profileDbDriverActivator == null)
                throw new ArgumentNullException("profileDbDriverActivator");

            _logger = logger;
            _nhibernateInfoProvider = nhibernateInfoProvider;
            _profileDbDriverFactory = profileDbDriverFactory;
            _profileDbDriverActivator = profileDbDriverActivator;
        }

        public void Inject()
        {
            _logger.Info("AdoPipelineInitiator for NH: Started wrapping the NHibernate DbDriver");

            var nhibernateAssembly = _nhibernateInfoProvider.GetNhibernateAssembly();
            var nhibernateDriverInfos = _nhibernateInfoProvider.GetNHibernateDriverInfos();
            if (nhibernateDriverInfos == null)
                return;

            _logger.Info(string.Format("AdoPipelineInitiator for NH: Found {0} drivers to wrap", nhibernateDriverInfos.Count()));

            foreach (var nhibernateDriverInfo in nhibernateDriverInfos)
            {
                if (nhibernateDriverInfo == null)
                    continue;

                // Check if the driver is already wrapped
                if (nhibernateDriverInfo.IsWrapped())
                    continue;

                // Get the profiled driver
                var profileDbDriverType = _profileDbDriverFactory.GetProfileDbDriverType(nhibernateAssembly);
                if (profileDbDriverType == null)
                    continue;

                var profileDbDriver = _profileDbDriverActivator.CreateProfileDbDriver(profileDbDriverType);
                if (profileDbDriver == null)
                    continue;

                // Wrap the driver with the profiled driver
                var driver = nhibernateDriverInfo.GetDriver();
                profileDbDriver.Wrap(driver);

                // Inject the profiled driver into nhibernate
                nhibernateDriverInfo.SetDriver(profileDbDriver);

                _logger.Info(string.Format("AdoPipelineInitiator for NH: Wrapped a driver ... " + Environment.NewLine +
                                           "Original driver type: {0}" + Environment.NewLine +
                                           "Profiled driver type: {1}", driver.GetType().FullName, profileDbDriver.GetType().FullName));
            }

            _logger.Info("AdoPipelineInitiator for NH: Finished wrapping the NHibernate DbDriver");
        }
    }
}