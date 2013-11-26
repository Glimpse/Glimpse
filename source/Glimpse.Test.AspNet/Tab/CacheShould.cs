using System;
using System.Web;
using System.Web.Caching;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class CacheShould
    {
        [Fact]
        public void CacheItems()
        {
            var contextMock = new Mock<ITabContext>();
            var cache = new Glimpse.AspNet.Tab.Cache();

            HttpRuntime.Cache.Add("testItem", "testItemValue", null, DateTime.MaxValue, new TimeSpan(2, 0, 0),
                      CacheItemPriority.AboveNormal, null);

            var result = cache.GetData(contextMock.Object);

            Assert.NotNull(result);
        }
    }
}
