using Glimpse.Core.Extensibility;

namespace Glimpse.WindowsAzure.Caching.Infrastructure
{
    public class DataCacheEventSubscriberFactoryInitializer
        : IInspector
    {
        public void Setup(IInspectorContext context)
        {
            DataCacheEventSubscriberFactory.SetOperationContextFactory(new GlimpseDataCacheEventSubscriber(context.MessageBroker, context.TimerStrategy));
        }
    }
}