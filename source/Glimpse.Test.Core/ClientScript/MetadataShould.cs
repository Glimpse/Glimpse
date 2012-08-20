using System.Collections.Generic;
using Glimpse.Core.ClientScript;
using Glimpse.Core.Extensibility;
using Xunit;

namespace Glimpse.Test.Core.ClientScript
{
    public class MetadataShould
    {
        [Fact]
        public void BeInTheProperOrder()
        {
            var clientScript = new Metadata();

            Assert.Equal(ScriptOrder.RequestMetadataScript, clientScript.Order);
        }

        [Fact]
        public void HaveProperResourceName()
        {
            var clientScript = new Metadata();

            Assert.Equal("glimpse_metadata", clientScript.GetResourceName());
        }

        [Fact]
        public void OverrideParameterValues()
        {
            var clientScript = new Metadata();

            var dictionary = new Dictionary<string, string>();

            clientScript.OverrideParameterValues(dictionary);

            Assert.True(dictionary.ContainsKey(ResourceParameter.Callback.Name));
        }
    }
}