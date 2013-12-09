using System;
using System.Web;
using System.Web.Caching;
using Glimpse.AspNet.Model;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class CacheShould
    {
        [Fact]
        public void CacheSlidingExpiration()
        {
            var contextMock = new Mock<ITabContext>();
            var cache = new Glimpse.AspNet.Tab.Cache();
            var slidingExpiration = new TimeSpan(2, 0, 0);

            HttpRuntime.Cache.Add("testItem", "testItemValue", null, Cache.NoAbsoluteExpiration, slidingExpiration,
                      CacheItemPriority.AboveNormal, null);

            var result = cache.GetData(contextMock.Object);

            Assert.NotNull(result);
            Assert.NotNull(result as CacheModel);

            var cacheModel = result as CacheModel;
           
            Assert.Equal(cacheModel.CacheItems.Count, 1);
            Assert.Equal(cacheModel.CacheItems[0].SlidingExpiration, slidingExpiration);
        }

        [Fact]
        public void CacheAbsoluteExpiration()
        {
            var contextMock = new Mock<ITabContext>();
            var cache = new Glimpse.AspNet.Tab.Cache();
            DateTime cacheExpiryDate = DateTime.Now.AddHours(6).ToUniversalTime();

            HttpRuntime.Cache.Add("testItem", "testItemValue", null, cacheExpiryDate, Cache.NoSlidingExpiration,
                      CacheItemPriority.AboveNormal, null);

            var result = cache.GetData(contextMock.Object);

            Assert.NotNull(result);
            Assert.NotNull(result as CacheModel);

            var cacheModel = result as CacheModel;

            Assert.Equal(cacheModel.CacheItems.Count, 1);

            var expiresOn = (DateTime)cacheModel.CacheItems[0].ExpiresOn;
            Assert.Equal(expiresOn.ToUniversalTime(), cacheExpiryDate);
        }
    }


}
