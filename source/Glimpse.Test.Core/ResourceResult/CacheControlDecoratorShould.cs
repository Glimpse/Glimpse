using Glimpse.Core.Extensibility;
// ReSharper disable RedundantUsingDirective
// Glimpse.Core.Extensions.EnumExtensions.ToDescription() is used in !DEBUG
using Glimpse.Core.Extensions;
// ReSharper restore RedundantUsingDirective
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.ResourceResult
{
    public class CacheControlDecoratorShould
    {
        [Theory, AutoMock]
        public void ConstructWithDefaults(IResourceResult resourceResult)
        {
            var sut = new CacheControlDecorator(resourceResult);

            Assert.False(sut.CacheSetting.HasValue);
        }

        [Theory, AutoMock]
        public void Construct(IResourceResult resourceResult)
        {
            var expectedDuration = 8;
            var expectedSetting = CacheSetting.NoStore;

            var sut = new CacheControlDecorator(expectedDuration, expectedSetting, resourceResult);

            Assert.Equal(expectedDuration, sut.CacheDuration);
            Assert.Equal(expectedSetting, sut.CacheSetting);
        }

        [Theory, AutoMock]
        public void ExecuteWrappedResource([Frozen] IResourceResult resourceResult, CacheControlDecorator sut, IResourceResultContext context)
        {
            sut.Execute(context);

            resourceResult.Verify(rr => rr.Execute(It.IsAny<IResourceResultContext>()));
        }

        [Theory, AutoMock]
        public void Execute(IResourceResult resourceResult, IResourceResultContext context)
        {
            var expectedDuration = 8;
            var expectedSetting = CacheSetting.NoStore;
            var sut = new CacheControlDecorator(expectedDuration, expectedSetting, resourceResult);

            sut.Execute(context);
#if !DEBUG
            var regex = string.Format(@"{0}.+{1}", expectedSetting.ToDescription(), expectedDuration);
            context.FrameworkProvider.Verify(fp => fp.SetHttpResponseHeader("Cache-Control", It.IsRegex(regex)));
#else
            context.FrameworkProvider.Verify(fp => fp.SetHttpResponseHeader("Cache-Control", "no-cache"));
#endif
        }

        [Fact]
        public void ExecuteInRelease()
        {
            Assert.True(true, "No way to test the non-debug release without rebuilding.");
        }
    }
}