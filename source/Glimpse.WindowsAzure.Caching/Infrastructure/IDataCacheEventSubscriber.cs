using Microsoft.ApplicationServer.Caching;

namespace Glimpse.WindowsAzure.Caching.Infrastructure
{
    public interface IDataCacheEventSubscriber
    {
        void Subscribe(DataCache cache);
        void Unsubscribe(DataCache cache);
    }
}