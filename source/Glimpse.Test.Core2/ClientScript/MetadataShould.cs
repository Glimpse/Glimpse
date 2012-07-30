using System.Collections.Generic;
using Glimpse.Core2.ClientScript;
using Glimpse.Core2.Extensibility;
using Xunit;

namespace Glimpse.Test.Core2.ClientScript
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

            Assert.Equal("glimpse-metadata", clientScript.GetResourceName());
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