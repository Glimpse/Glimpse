using Glimpse.AspNet.Net35.Backport;
using Xunit;

namespace Glimpse.Test.AspNet.Net35
{
    public class Net35BackportShould
    {
        [Fact]
        public void HandleIsNullOrWhiteSpace()
        {
            Assert.True(Net35Backport.IsNullOrWhiteSpace(null));
            Assert.True(Net35Backport.IsNullOrWhiteSpace("       "));
            Assert.False(Net35Backport.IsNullOrWhiteSpace("any string"));
        }
    }
}
