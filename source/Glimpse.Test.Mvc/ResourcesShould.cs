using System.Globalization;
using Glimpse.Mvc;
using Xunit;

namespace Glimpse.Test.Mvc
{
    public class ResourcesShould
    {
        [Fact]
        public void HaveResources()
        {
            Assert.NotNull(Resources.FindViewsProxyOutputReplacedIView);
            Assert.NotNull(Resources.ResourceManager);
            Assert.NotNull(Resources.ViewEngineSetupReplacedViewEngine);
        }

        [Fact]
        public void Construct()
        {
            Assert.NotNull(new Resources());
        }

        [Fact]
        public void ChangeCulture()
        {
            Assert.Null(Resources.Culture);

            var us = new CultureInfo("en-US");

            Resources.Culture = us;

            Assert.Equal(us, Resources.Culture);
        }
    }
}