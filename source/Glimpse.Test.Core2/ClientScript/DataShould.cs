using Glimpse.Core2.ClientScript;
using Glimpse.Core2.Extensibility;
using Xunit;

namespace Glimpse.Test.Core2.ClientScript
{
    public class DataShould
    {
        [Fact]
        public void BeARequestDataScript()
        {
            var clientScript = new Data();
            Assert.Equal(ScriptOrder.RequestDataScript, clientScript.Order);
        }

        [Fact]
        public void HaveProperResourceName()
        {
            var clientScript = new Data();
            Assert.Equal("data.js", clientScript.GetResourceName());
        }

        [Fact]
        public void BeDynamic()
        {
            Assert.NotNull(new Data() as IDynamicClientScript);
        }
    }
}