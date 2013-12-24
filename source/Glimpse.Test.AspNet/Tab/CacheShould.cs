using System;
using System.Web;
using System.Web.Caching;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class CacheShould : IDisposable
    {
        private const string CacheItemKey = "testItemKey";
        private const string CacheItemValue = "testItemValue";

        [Fact]
        public void ReturnData()
        {
            var contextMock = new Mock<ITabContext>();
            var cache = new Glimpse.AspNet.Tab.Cache();
            var slidingExpiration = new TimeSpan(2, 0, 0);

            HttpRuntime.Cache.Add(CacheItemKey, CacheItemValue, null, Cache.NoAbsoluteExpiration, slidingExpiration,
                      CacheItemPriority.AboveNormal, null);

            var cacheModel = cache.GetData(contextMock.Object) as CacheModel;

            Assert.NotNull(cacheModel);
            Assert.Equal(cacheModel.CacheItems[0].Key, CacheItemKey);
            Assert.Equal(cacheModel.CacheItems[0].Value, CacheItemValue);
        }

        [Fact]
        public void HaveSlidingExpiration()
        {
            var contextMock = new Mock<ITabContext>();
            var cache = new Glimpse.AspNet.Tab.Cache();
            var slidingExpiration = new TimeSpan(2, 0, 0);

            HttpRuntime.Cache.Add(CacheItemKey, CacheItemValue, null, Cache.NoAbsoluteExpiration, slidingExpiration,
                      CacheItemPriority.AboveNormal, null);

            var cacheModel = cache.GetData(contextMock.Object) as CacheModel;

            Assert.NotNull(cacheModel);
           
            Assert.Equal(cacheModel.CacheItems.Count, 1);
            Assert.Equal(cacheModel.CacheItems[0].SlidingExpiration, slidingExpiration);
        }

        [Fact]
        public void HaveAbsoluteExpiration()
        {
            var contextMock = new Mock<ITabContext>();
            var cache = new Glimpse.AspNet.Tab.Cache();
            var cacheExpiryDate = DateTime.Now.AddHours(6).ToUniversalTime();

            HttpRuntime.Cache.Add(CacheItemKey, CacheItemValue, null, cacheExpiryDate, Cache.NoSlidingExpiration,
                      CacheItemPriority.AboveNormal, null);

            var cacheModel = cache.GetData(contextMock.Object) as CacheModel;

            Assert.NotNull(cacheModel);

            Assert.Equal(cacheModel.CacheItems.Count, 1);

            var expiresOn = cacheModel.CacheItems[0].ExpiresOn;
            Assert.Equal(expiresOn, cacheExpiryDate);
        }
        
        public void Dispose()
        {
            HttpRuntime.Cache.Remove(CacheItemKey);
        }
    }
}
