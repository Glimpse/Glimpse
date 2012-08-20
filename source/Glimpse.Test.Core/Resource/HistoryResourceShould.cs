using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Resource
{
    public class HistoryResourceShould
    {
        [Fact]
        public void ContainATopParameter()
        {
            var resource = new HistoryResource();

            Assert.Equal("top", resource.Parameters.First().Name);
        }

        [Fact]
        public void ShouldHaveProperName()
        {
            var resource = new HistoryResource();

            Assert.Equal("glimpse_history", resource.Name);
        }

        [Fact]
        public void ThrowExceptionWithNullContext()
        {
            var resource = new HistoryResource();

            Assert.Throws<ArgumentNullException>(()=>resource.Execute(null));
        }

        [Theory]
        [InlineData("top", "non-integer", false, 50)]
        [InlineData("top", "10", true, 10)]
        [InlineData("wrong-key", "non-integer", false, 50)]
        [InlineData("wrong-key", "10", true, 50)]
        public void AcquireTopParameter(string parameterName, string parameterValue, bool parameterValueIsInt, int expectedResult)
        {
            var resource = new HistoryResource();

            var storeMock = new Mock<IReadOnlyPersistanceStore>();
            var contextMock = new Mock<IResourceContext>();
            string output = parameterValue;
            contextMock.Setup(c => c.Parameters.TryGetValue(parameterName, out output)).Returns(parameterValueIsInt);
            contextMock.Setup(c => c.PersistanceStore).Returns(storeMock.Object);

            resource.Execute(contextMock.Object);

            storeMock.Verify(s=>s.GetTop(expectedResult));
        }

        [Fact]
        public void Return404StatusCodeWithoutData()
        {
            var resource = new HistoryResource();

            var storeMock = new Mock<IReadOnlyPersistanceStore>();
            storeMock.Setup(s => s.GetTop(It.IsAny<int>())).Returns<IEnumerable<GlimpseRequest>>(null);
            var contextMock = new Mock<IResourceContext>();
            string output = "25";
            contextMock.Setup(c => c.Parameters.TryGetValue("top", out output)).Returns(true);
            contextMock.Setup(c => c.PersistanceStore).Returns(storeMock.Object);

            var result = resource.Execute(contextMock.Object);

            var statusCodeResult = result as StatusCodeResourceResult;

            Assert.NotNull(statusCodeResult);
            Assert.Equal(404, statusCodeResult.StatusCode);
        }

        [Fact]
        public void ReturnJsonResourceResultWithData()
        {
            var resource = new HistoryResource();

            var storeMock = new Mock<IReadOnlyPersistanceStore>();
            storeMock.Setup(s => s.GetTop(It.IsAny<int>())).Returns(Enumerable.Empty<GlimpseRequest>());
            var contextMock = new Mock<IResourceContext>();
            string output = "25";
            contextMock.Setup(c => c.Parameters.TryGetValue("top", out output)).Returns(true);
            contextMock.Setup(c => c.PersistanceStore).Returns(storeMock.Object);

            var result = resource.Execute(contextMock.Object);

            var jsonResourceResult = result as JsonResourceResult;

            Assert.NotNull(jsonResourceResult);
        }
    }
}