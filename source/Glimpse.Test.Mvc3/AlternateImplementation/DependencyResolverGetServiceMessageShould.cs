using Glimpse.Mvc.AlternateImplementation;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServiceMessageShould
    {
        [Fact]
        public void Construct()
        {
            var input = typeof(string);
            var output = "output";

            var message = new DependencyResolver.GetService.Message(input, output);

            Assert.Equal(input, message.ServiceType);
            Assert.Equal(output.GetType(), message.ResolvedType);
            Assert.True(message.IsResolved);
        }

        [Fact]
        public void HandleNullResolvedObjects()
        {
            var input = typeof(string);
            object output = null;

            var message = new DependencyResolver.GetService.Message(input, output);

            Assert.Equal(input, message.ServiceType);
            Assert.Equal(null, message.ResolvedType);
            Assert.False(message.IsResolved);
        }
    }
}