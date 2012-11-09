using System;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewEngineFindViewsMessageShould
    {
        [Theory, AutoMock]
        public void NotFindViewWithMissingViewEngineResult(ViewEngine.FindViews.Arguments input, TimerResult timing, Type baseType, bool isPartial, Guid id)
        {
            var output = new ViewEngineResult(Enumerable.Empty<string>());

            var sut = new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id);

            Assert.Equal(input, sut.Input);
            Assert.Equal(output, sut.Output);
            Assert.Equal(timing, sut.Timing);
            Assert.Equal(isPartial, sut.IsPartial);
            Assert.Equal(id, sut.Id);
            Assert.False(sut.IsFound);
        }

        [Theory, AutoMock]
        public void FindViewWithViewEngineResult(ViewEngine.FindViews.Arguments input, TimerResult timing, Type baseType, bool isPartial, Guid id)
        {
            var output = new ViewEngineResult(new Mock<IView>().Object, new Mock<IViewEngine>().Object);

            var sut = new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id);

            Assert.Equal(input, sut.Input);
            Assert.Equal(output, sut.Output);
            Assert.Equal(timing, sut.Timing);
            Assert.Equal(isPartial, sut.IsPartial);
            Assert.Equal(id, sut.Id);
            Assert.True(sut.IsFound);
        }

        [Theory, AutoMock]
        public void ThrowArgumentExceptionWithNullInput(ViewEngineResult output, TimerResult timing, Type baseType, bool isPartial, Guid id)
        {
            Assert.Throws<ArgumentNullException>(() => new ViewEngine.FindViews.Message(null, output, timing, baseType, isPartial, id));
        }

        [Theory, AutoMock]
        public void ThrowArgumentExceptionWithNullOutput(ViewEngine.FindViews.Arguments input, TimerResult timing, Type baseType, bool isPartial, Guid id)
        {
            Assert.Throws<ArgumentNullException>(() => new ViewEngine.FindViews.Message(input, null, timing, baseType, isPartial, id));
        }

        [Theory, AutoMock]
        public void ThrowArgumentExceptionWithNullTiming(ViewEngine.FindViews.Arguments input, ViewEngineResult output, Type baseType, bool isPartial, Guid id)
        {
            Assert.Throws<ArgumentNullException>(() => new ViewEngine.FindViews.Message(input, output, null, baseType, isPartial, id));
        }
    }
}