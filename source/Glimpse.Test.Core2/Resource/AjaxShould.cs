using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;
using Glimpse.Core2.Resource;
using Glimpse.Core2.ResourceResult;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Resource
{
    public class AjaxShould
    {
        [Fact]
        public void ProvideProperName()
        {
            var resource = new AjaxResource();
            Assert.Equal("glimpse_ajax", resource.Name);
        }

        [Fact]
        public void ReturnThreeParameterKeys()
        {
            var resource = new AjaxResource();
            Assert.Equal(3, resource.Parameters.Count());
        }

        [Fact]
        public void ThrowExceptionWithNullParameters()
        {
            var resource = new AjaxResource();

            Assert.Throws<ArgumentNullException>(() => resource.Execute(null));
        }

        [Fact]
        public void ReturnStatusCodeResourceResultWithMissingParameter()
        {
            var contextMock = new Mock<IResourceContext>();
            contextMock.Setup(c => c.Parameters).Returns(new Dictionary<string, string> { {"ParentRequestId", "bad data"} });

            var resource = new AjaxResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as StatusCodeResourceResult);
        }

        [Fact]
        public void ReturnJsonResourceResult()
        {
            var contextMock = new Mock<IResourceContext>();

            var guid = Guid.NewGuid();

            contextMock.Setup(c => c.Parameters["ParentRequestId"]).Returns(guid.ToString());
            contextMock.Setup(c => c.PersistanceStore.GetByRequestParentId(guid)).Returns(Enumerable.Empty<GlimpseRequest>());

            var resource = new AjaxResource();

            var result = resource.Execute(contextMock.Object);

            Assert.NotNull(result as JsonResourceResult);
        }
    }
}