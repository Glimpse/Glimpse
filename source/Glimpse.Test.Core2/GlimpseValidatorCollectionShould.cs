using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseValidatorCollectionShould
    {
        [Fact]
        public void Construct()
        {
            var validators = new GlimpseValidatorCollection();

            Assert.NotNull(validators);
        }

        [Fact]
        public void AutoDiscover()
        {
            var validators = new GlimpseValidatorCollection();

            validators.Discoverability.Discover();

            Assert.Equal(1, validators.Count);
        }

        [Fact]
        public void ReturnModeOfOnWithoutValidators()
        {
            var validators = new GlimpseValidatorCollection();

            Assert.Equal(GlimpseMode.On, validators.GetMode(null));
        }

        [Fact]
        public void ReturnModeFromValidators()
        {
            var validators = new GlimpseValidatorCollection();

            validators.Discoverability.Discover();

            Assert.Equal(GlimpseMode.Body, validators.GetMode(null));
        }

        [Fact]
        public void ReturnModeOfLeastPrivilege()
        {
            var validators = new GlimpseValidatorCollection();

            validators.Discoverability.Discover();

            var mockValidator = new Mock<IGlimpseValidator>();
            mockValidator.Setup(v => v.GetMode(null)).Returns(GlimpseMode.Silent);

            validators.Add(mockValidator.Object);

            Assert.Equal(GlimpseMode.Silent, validators.GetMode(null));
            mockValidator.Verify(v=>v.GetMode(null), Times.AtLeastOnce());
        }
    }

    [GlimpseValidator]
    public class TestValidator : IGlimpseValidator
    {
        public GlimpseMode GetMode(RequestMetadata requestMetadata)
        {
            return GlimpseMode.Body;
        }
    }

}