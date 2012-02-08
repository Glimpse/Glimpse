using System;
using Glimpse.Core2;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.Tester;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Framework
{
    public class GlimpseConfigurationShould : IDisposable
    {
        private GlimpseConfigurationTester tester { get; set; }

        private GlimpseConfigurationTester Configuration
        {
            get { return tester ?? (tester = GlimpseConfigurationTester.Create()); }
            set { tester = value; }
        }

        [Fact]
        public void CreateDefaultHtmlEncoder()
        {
            Assert.NotNull(Configuration.HtmlEncoder);
        }

        [Fact]
        public void CreateDefaultLogger()
        {
            Assert.NotNull(Configuration.Logger);
        }

        [Fact(Skip = "Create a test to cover the constructor")]
        public void Construct()
        {
            
        }


        [Fact]
        public void CreateDefaultPiplineInspectorsCollection()
        {
            Assert.NotNull(Configuration.PipelineInspectors);
        }

        [Fact]
        public void CreateDefaultResourcesCollection()
        {
            Assert.NotNull(Configuration.Resources);
        }

        [Fact]
        public void CreateDefaultSerializer()
        {
            Assert.NotNull(Configuration.Serializer);
        }

        [Fact]
        public void CreateDefaultTabsCollection()
        {
            Assert.NotNull(Configuration.Tabs);
        }

        [Fact]
        public void CreateDefaultValidatorsCollection()
        {
            Assert.NotNull(Configuration.RuntimePolicies);
        }

        [Fact]
        public void CreateDefaultClientScripts()
        {
            Assert.NotNull(Configuration.ClientScriptsStub);
        }

        [Fact]
        public void CreateDefaultResourceName()
        {
            Assert.True(!string.IsNullOrEmpty(Configuration.DefaultResourceName));
        }

        [Fact]
        public void NotDiscoverPipelineInspectors()
        {
            Assert.Equal(0, Configuration.PipelineInspectors.Count);
        }

        [Fact]
        public void NotDiscoverResources()
        {
            Assert.Equal(0, Configuration.Resources.Count);
        }

        [Fact]
        public void NotDiscoverTabs()
        {
            Assert.Equal(0, Configuration.Tabs.Count);
        }

        [Fact]
        public void NotDiscoverValidators()
        {
            Assert.Equal(0, Configuration.RuntimePolicies.Count);
        }

        [Fact(Skip = "Test all Contract.Requires clauses in GlimpseConfiguration ctor")]
        public void ThrowExceptionWhenConstructedWithNullEndpointConfiguration()
        {
        }


        [Fact]
        public void FrameworkProviderCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.FrameworkProvider = null);
        }

        [Fact]
        public void HtmlEncoderCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.HtmlEncoder = null);
        }

        [Fact]
        public void LogerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Logger = null);
        }

        [Fact]
        public void PersistanceStoreCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.PersistanceStore = null);
        }

        [Fact]
        public void PipelineInspectorsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.PipelineInspectors = null);
        }

        [Fact]
        public void ResourceEndpointCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ResourceEndpoint = null);
        }

        [Fact]
        public void ResourcesCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Resources = null);
        }

        [Fact]
        public void SerializerCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Serializer = null);
        }

        [Fact]
        public void TabsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.Tabs = null);
        }

        [Fact]
        public void ValidatorsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.RuntimePolicies = null);
        }

        [Fact]
        public void ClientScriptsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ClientScripts = null);
        }

        [Fact]
        public void DefaultResourceNameCannotBeNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.DefaultResourceName = null);
            Assert.Throws<ArgumentNullException>(() => Configuration.DefaultResourceName = "");
        }

        [Fact]
        public void ChangeGlimpseMode()
        {
            Configuration.BaseRuntimePolicy = RuntimePolicy.ModifyResponseBody;

            Assert.Equal(RuntimePolicy.ModifyResponseBody, Configuration.BaseRuntimePolicy);
        }

        public void Dispose()
        {
            Configuration = null;
        }
    }
}