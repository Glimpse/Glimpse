using Microsoft.ApplicationServer.Caching;

namespace Glimpse.WindowsAzure.Caching.Infrastructure
{
    public class DefaultCacheEventSubscriber
        : IDataCacheEventSubscriber
    {
        public void Subscribe(DataCache cache)
        {
            // noop
        }

        public void Unsubscribe(DataCache cache)
        {
            // noop
        }
    }
}