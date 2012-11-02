using System.IO;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewRenderMessageShould
    {
        [Fact]
        public void SetProperties()
        {
            var viewContext = new ViewContext();
            var textWriter = new StringWriter();
            var arguments = new View.Render.Arguments(new object[] { viewContext, textWriter });

            var timerResult = new TimerResult();
            var baseType = typeof (ViewRenderMessageShould);

            var mixinMock = new Mock<IViewCorrelationMixin>();
            var mixin = mixinMock.Object;

            var message = new View.Render.Message(arguments, timerResult, baseType, mixin);

            Assert.Equal(arguments, message.Input);
            Assert.Equal(timerResult, message.Timing);
            Assert.Equal(baseType, message.BaseType);
            Assert.Equal(mixin, message.ViewCorrelation);
        }
    }
}