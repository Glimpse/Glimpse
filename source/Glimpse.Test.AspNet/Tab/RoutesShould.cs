using Glimpse.AspNet.Tab;
using Xunit;

namespace Glimpse.Test.AspNet.Tab
{
    public class RoutesShould
    {
        [Fact]
        public void ReturnName()
        {
            var tab = new Routes();

            Assert.Equal("Routes", tab.Name);
        }

        [Fact]
        public void ReturnDocumentationUri()
        {
            var tab = new Routes();

            Assert.True(tab.DocumentationUri.Contains("getGlimpse.com"));
        }
    }
}