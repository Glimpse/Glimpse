using System.Web.Mvc; 
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common; 
using Ploeh.AutoFixture.Xunit; 
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewRenderMessageShould
    {
        [Theory, AutoMock]
        public void SetProperties(View.Render.Arguments arguments, TimerResult timerResult, IViewCorrelationMixin mixin)
        {
            var baseType = typeof(ViewRenderMessageShould);

            var sut = new View.Render.Message(arguments, typeof(IView), null, timerResult, baseType, mixin);

            Assert.Equal(arguments, sut.Input);
            Assert.Equal(timerResult.Duration, sut.Duration);
            Assert.Equal(timerResult.StartTime, sut.StartTime);
            Assert.Equal(timerResult.Offset, sut.Offset);
            Assert.Equal(baseType, sut.BaseType);
            Assert.Equal(mixin, sut.ViewCorrelation);
        }

        [Theory, AutoMock]
        public void SetInvalidModelState(View.Render.Arguments arguments, TimerResult timerResult, IViewCorrelationMixin mixin)
        {
            arguments.ViewContext.ViewData.ModelState.AddModelError("key", @"there was an error");

            var sut = new View.Render.Message(arguments, typeof(IView), null, timerResult, typeof(ViewRenderMessageShould), mixin);

            Assert.False(sut.ModelStateIsValid);
        }

        [Theory, AutoMock]
        public void SetValidModelState([Frozen]ViewDataDictionary viewData, View.Render.Arguments arguments, TimerResult timerResult, IViewCorrelationMixin mixin)
        {
            var sut = new View.Render.Message(arguments, typeof(IView), null, timerResult, typeof(ViewRenderMessageShould), mixin);

            Assert.True(sut.ModelStateIsValid);
        }

        [Theory, AutoMock]
        public void SetEmptyModel(View.Render.Arguments arguments, TimerResult timerResult, IViewCorrelationMixin mixin)
        {
            var sut = new View.Render.Message(arguments, typeof(IView), null, timerResult, typeof(ViewRenderMessageShould), mixin);

            Assert.Null(sut.ViewDataModelType);
        }

        [Theory, AutoMock]
        public void SetValidModel([Frozen]ViewDataDictionary viewData, View.Render.Arguments arguments, TimerResult timerResult, IViewCorrelationMixin mixin)
        {
            arguments.ViewContext.ViewData.Model = new object();

            var sut = new View.Render.Message(arguments, typeof(IView), null, timerResult, typeof(ViewRenderMessageShould), mixin);

            Assert.Equal(typeof(object), sut.ViewDataModelType);
        }
    }
}