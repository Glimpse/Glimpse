using System.Collections.Generic;
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
            Assert.Equal("glimpse_request", clientScript.GetResourceName());
        }

        [Fact]
        public void BeDynamic()
        {
            Assert.NotNull(new Data() as IDynamicClientScript);
        }

        [Fact]
        public void OverrideParameterValues()
        {
            var clientScript = new Data();

            var dictionary = new Dictionary<string, string>();

            clientScript.OverrideParameterValues(dictionary);

            Assert.True(dictionary.ContainsKey(ResourceParameter.Callback.Name));
        }
    }
}