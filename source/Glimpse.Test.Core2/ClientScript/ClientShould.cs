using Glimpse.Core2.ClientScript;
using Glimpse.Core2.Extensibility;
using Xunit;

namespace Glimpse.Test.Core2.ClientScript
{
    public class ClientShould
    {
        [Fact]
        public void BeARequestDataScript()
        {
            var clientScript = new Client();
            Assert.Equal(ScriptOrder.ClientInterfaceScript, clientScript.Order);
        }

        [Fact]
        public void HaveProperResourceName()
        {
            var clientScript = new Client();
            Assert.Equal("glimpse.js", clientScript.GetResourceName());
        }

        [Fact]
        public void BeDynamic()
        {
            Assert.NotNull(new Client() as IDynamicClientScript);
        }
    }
}