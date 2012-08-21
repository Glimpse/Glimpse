using System;
using System.Linq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServicesMessageShould
    {
        [Fact]
        public void Construct()
        {
            var input = typeof (string);
            var output = new object[] {1, "a", DateTime.Now};

            var message = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetServices.Message(input, output);

            Assert.Equal(input, message.ServiceType);
            Assert.Equal(output.Length, message.ResolvedTypes.Count());
            Assert.True(message.IsResolved);
        }

        [Fact]
        public void HandleEmptyOutput()
        {
            var input = typeof(string);
            var output = Enumerable.Empty<object>();

            var message = new Glimpse.Mvc3.AlternateImplementation.DependencyResolver.GetServices.Message(input, output);

            Assert.Equal(input, message.ServiceType);
            Assert.Null(message.ResolvedTypes);
            Assert.False(message.IsResolved);
        }
    }
}