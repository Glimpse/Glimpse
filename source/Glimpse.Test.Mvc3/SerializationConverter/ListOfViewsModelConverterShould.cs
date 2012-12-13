using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.SerializationConverter;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.AlternateImplementation;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.SerializationConverter
{
    public class ListOfViewsModelConverterShould
    {
        [Theory, AutoMock]
        public void ConvertEmptyCollection(ListOfViewsModelConverter sut)
        {
            var result = sut.Convert(new List<ViewsModel>()) as IEnumerable<object>;

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory, AutoMock]
        public void ConvertCollection(ListOfViewsModelConverter sut, ViewEngine.FindViews.Arguments findViewArgs, View.Render.Arguments renderArgs, TimerResult timerResult, IViewCorrelationMixin mixin)
        {
            var findViewsMessage1 = new ViewEngine.FindViews.Message(findViewArgs, timerResult, typeof(IViewEngine), null, new ViewEngineResult(Enumerable.Empty<string>()), typeof(string), false, Guid.NewGuid());
            var findViewsMessage2 = new ViewEngine.FindViews.Message(findViewArgs, timerResult, typeof(IViewEngine), null, new ViewEngineResult(new Mock<IView>().Object, new Mock<IViewEngine>().Object), typeof(string), false, Guid.NewGuid());
            var findViewsMessage3 = new ViewEngine.FindViews.Message(findViewArgs, timerResult, typeof(IViewEngine), null, new ViewEngineResult(Enumerable.Empty<string>()), typeof(string), false, Guid.NewGuid());

            var renderMessage = new View.Render.Message(renderArgs, typeof(IView), null, timerResult, typeof(ViewRenderMessageShould), mixin);

            var vms = new List<ViewsModel>
                {
                    new ViewsModel(findViewsMessage1, renderMessage), 
                    new ViewsModel(findViewsMessage2, renderMessage), 
                    new ViewsModel(findViewsMessage3, renderMessage)
                };

            var result = sut.Convert(vms) as IEnumerable<object>;

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}