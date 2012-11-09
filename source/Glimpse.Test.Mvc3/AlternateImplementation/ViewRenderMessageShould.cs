using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
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

            var sut = new View.Render.Message(arguments, timerResult, baseType, mixin);

            Assert.Equal(arguments, sut.Input);
            Assert.Equal(timerResult, sut.Timing);
            Assert.Equal(baseType, sut.BaseType);
            Assert.Equal(mixin, sut.ViewCorrelation);
        }
    }
}