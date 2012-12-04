using System;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class DependencyResolverGetServiceMessageShould
    {
        [Theory, AutoMock]
        public void Construct(Type input, string output)
        {
            var sut = new DependencyResolver.GetService.Message(typeof(System.Web.Mvc.IDependencyResolver), null, input, output);

            Assert.Equal(input, sut.ServiceType);
            Assert.Equal(output.GetType(), sut.ResolvedType);
            Assert.True(sut.IsResolved);
        }

        [Theory, AutoMock]
        public void HandleNullResolvedObjects(Type input)
        {
            var sut = new DependencyResolver.GetService.Message(typeof(System.Web.Mvc.IDependencyResolver), null, input, null);

            Assert.Equal(input, sut.ServiceType);
            Assert.Equal(null, sut.ResolvedType);
            Assert.False(sut.IsResolved);
        }
    }
}