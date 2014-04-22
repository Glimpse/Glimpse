using System;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    internal class InspectorProvider : BaseProvider
    {
        public InspectorProvider(IConfiguration configuration, ActiveGlimpseRequestContexts activeGlimpseRequestContexts)
            : base(configuration, activeGlimpseRequestContexts)
        { 
        }

        public void Setup()
        {
            var logger = Configuration.Logger;
            var messageBroker = Configuration.MessageBroker;

            var inspectorContext = new InspectorContext(logger, Configuration.ProxyFactory, messageBroker, () => ActiveGlimpseRequestContexts.Current.CurrentExecutionTimer, () => ActiveGlimpseRequestContexts.Current.CurrentRuntimePolicy);
            foreach (var inspector in Configuration.Inspectors)
            {
                try
                {
                    inspector.Setup(inspectorContext);
                    logger.Debug(Resources.GlimpseRuntimeInitializeSetupInspector, inspector.GetType());
                }
                catch (Exception exception)
                {
                    logger.Error(Resources.InitializeInspectorError, exception, inspector.GetType());
                }
            }
        }
    }
}