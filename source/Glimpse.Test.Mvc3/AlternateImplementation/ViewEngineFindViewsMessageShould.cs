using System;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewEngineFindViewsMessageShould
    {
        [Fact]
        public void NotFindViewWithMissingViewEngineResult()
        {
            var input = new ViewEngine.FindViews.Arguments(new object[] {new ControllerContext(), "ViewName", false}, true);
            var output = new ViewEngineResult(Enumerable.Empty<string>());
            var timing = new TimerResult();
            var baseType = typeof (string);
            var isPartial = false;
            var id = Guid.NewGuid();

            var message = new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id);

            Assert.Equal(input, message.Input);
            Assert.Equal(output, message.Output);
            Assert.Equal(timing, message.Timing);
            Assert.Equal(isPartial, message.IsPartial);
            Assert.Equal(id, message.Id);
            Assert.False(message.IsFound);
        }

        [Fact]
        public void FindViewWIthViewEngineResult()
        {
            var viewMock = new Mock<IView>();
            var viewEngineMock = new Mock<IViewEngine>();

            var input = new ViewEngine.FindViews.Arguments(new object[] { new ControllerContext(), "ViewName", "MasterName", false }, false);
            var output = new ViewEngineResult(viewMock.Object, viewEngineMock.Object);
            var timing = new TimerResult();
            var baseType = typeof(string);
            var isPartial = false;
            var id = Guid.NewGuid();

            var message = new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id);

            Assert.Equal(input, message.Input);
            Assert.Equal(output, message.Output);
            Assert.Equal(timing, message.Timing);
            Assert.Equal(isPartial, message.IsPartial);
            Assert.Equal(id, message.Id);
            Assert.True(message.IsFound);
        }

        [Fact]
        public void ThrowArgumentExceptionWithNullInput()
        {
            ViewEngine.FindViews.Arguments input = null;
            var output = new ViewEngineResult(Enumerable.Empty<string>());
            var timing = new TimerResult();
            var baseType = typeof(string);
            var isPartial = false;
            var id = Guid.NewGuid();

            Assert.Throws<ArgumentNullException>(()=> new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id));
        }

        [Fact]
        public void ThrowArgumentExceptionWithNullOutput()
        {
            var input = new ViewEngine.FindViews.Arguments(new object[] { new ControllerContext(), "ViewName", false }, true);
            ViewEngineResult output = null;
            var timing = new TimerResult();
            var baseType = typeof(string);
            var isPartial = false;
            var id = Guid.NewGuid();

            Assert.Throws<ArgumentNullException>(() => new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id));
        }

        [Fact]
        public void ThrowArgumentExceptionWithNullTiming()
        {
            var input = new ViewEngine.FindViews.Arguments(new object[] { new ControllerContext(), "ViewName", false }, true);
            var output = new ViewEngineResult(Enumerable.Empty<string>());
            TimerResult timing = null;
            var baseType = typeof(string);
            var isPartial = false;
            var id = Guid.NewGuid();

            Assert.Throws<ArgumentNullException>(() => new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id));
        }
    }
}