using System;
using System.Globalization;
using Glimpse.Core;
using Xunit;

namespace Glimpse.Test.Core
{
    public class ResourcesShould:IDisposable
    {
        //These tests aren't really needed. I was just being a tad anal when combing through coverage percentages.

        [Fact]
        public void ChangeCulture()
        {
            Assert.Null(Resources.Culture);

            var us = new CultureInfo("en-US");

            Resources.Culture = us;

            Assert.Equal(us,  Resources.Culture);
        }

        [Fact]
        public void GetResourceManager()
        {
            Assert.NotNull(Resources.ResourceManager);
        }

        [Fact]
        public void ResourcesHaveValues()
        {
            Assert.NotNull(Resources.ExecutePolicyWarning);
            Assert.NotNull(Resources.ExecuteResourceDuplicateError);
            Assert.NotNull(Resources.ExecuteResourceMissingError);
            Assert.NotNull(Resources.ExecuteTabError);
            Assert.NotNull(Resources.InitializePipelineInspectorError);
            Assert.NotNull(Resources.InitializeTabError);
            Assert.NotNull(Resources.EndRequestOutOfOrderRuntimeMethodCall);
            Assert.NotNull(Resources.RenderClientScriptImproperImplementationWarning);
            Assert.NotNull(Resources.RenderClientScriptMissingResourceWarning);
            Assert.NotNull(Resources.GenerateUriParameterKeysWarning);
        }

        [Fact]
        public void ResourceRemainTheSameAcrossCultures()
        {
            Resources.Culture = new CultureInfo("en-US");
            var enWarn = Resources.ExecutePolicyWarning;

            Resources.Culture = new CultureInfo("ja-JP");
            var jpWarn = Resources.ExecutePolicyWarning;

            Assert.Equal(enWarn, jpWarn);
        }

        [Fact]
        public void Construct()
        {
            Assert.NotNull(new Resources());
        }

        public void Dispose()
        {
            Resources.Culture = null;
        }
    }
}