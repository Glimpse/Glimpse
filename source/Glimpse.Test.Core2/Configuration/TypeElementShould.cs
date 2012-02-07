using Glimpse.Core2.Configuration;
using Xunit;

namespace Glimpse.Test.Core2.Configuration
{
    public class TypeElementShould
    {
        [Fact]
        public void NotSetNull()
        {
            var type = typeof (TypeElementShould);
            var element = new TypeElement();
            element.Type = type;

            Assert.Equal(type, element.Type);
        }
    }
}