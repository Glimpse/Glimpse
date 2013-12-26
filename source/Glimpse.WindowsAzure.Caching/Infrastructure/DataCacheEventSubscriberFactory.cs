namespace Glimpse.WindowsAzure.Caching.Infrastructure
{
    public static class DataCacheEventSubscriberFactory
    {
        static DataCacheEventSubscriberFactory()
        {
            Current = new DefaultCacheEventSubscriber();
        }

        public static void SetOperationContextFactory(IDataCacheEventSubscriber factory)
        {
            lock (Current)
            {
                Current = factory;
            }
        }

        public static IDataCacheEventSubscriber Current { get; private set; }
    }
}
