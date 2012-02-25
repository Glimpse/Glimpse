using System.Web;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.Tab;
using Xunit;

namespace Glimpse.Test.Mvc.Tab
{
    public class ViewsShould
    {
        [Fact]
        public void Construct()
        {
            var views = new Views();

            Assert.NotNull(views as ITab);
            Assert.NotNull(views as ITabSetup);
        }

        [Fact]
        public void ExecuteOnEndRequest()
        {
            var views = new Views();

            Assert.Equal(RuntimeEvent.EndRequest, views.ExecuteOn);
        }

        [Fact]
        public void HaveHttpContextBase()
        {
            var views = new Views();
            Assert.Equal(typeof(HttpContextBase), views.RequestContextType);
        }
    }
}