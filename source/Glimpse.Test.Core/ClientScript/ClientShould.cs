using Glimpse.Core.ClientScript;
using Glimpse.Core.Extensibility;
using Xunit;

namespace Glimpse.Test.Core.ClientScript
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
            Assert.Equal("glimpse_client", clientScript.GetResourceName());
        }

        [Fact]
        public void BeDynamic()
        {
            Assert.NotNull(new Client() as IDynamicClientScript);
        }
    }
}