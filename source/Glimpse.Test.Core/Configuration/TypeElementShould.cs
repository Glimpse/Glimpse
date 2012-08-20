using Glimpse.Core.Configuration;
using Xunit;

namespace Glimpse.Test.Core.Configuration
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