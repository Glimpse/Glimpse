using Glimpse.Core.Extensions;
using Glimpse.Test.Core.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core.Extensions
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