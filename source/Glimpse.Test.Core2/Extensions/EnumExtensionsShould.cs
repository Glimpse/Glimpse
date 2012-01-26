using Glimpse.Core2.Extensions;
using Glimpse.Test.Core2.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core2.Extensions
{
    public class EnumExtensionsShould
    {
        [Fact]
        public void ReturnDescriptWhenItExists()
        {
            Assert.Equal("I am described", DummyEnum.WithDescription.ToDescription());
        }

        [Fact]
        public void ReturnEmptyStringWithItDoesNotExist()
        {
            Assert.Empty(DummyEnum.WithoutDescription.ToDescription());
        }
    }
}