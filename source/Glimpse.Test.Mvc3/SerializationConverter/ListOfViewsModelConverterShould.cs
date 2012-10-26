using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.SerializationConverter;
using Glimpse.Test.Mvc3.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.SerializationConverter
{
    public class ListOfViewsModelConverterShould
    {
        [Fact]
        public void ConvertEmptyCollection()
        {
            var emptyCollection = new List<ViewsModel>();

            var converter = new ListOfViewsModelConverter();
            var result = converter.Convert(emptyCollection);

            var data = result as List<IEnumerable<object>>;

            Assert.NotNull(data);
            Assert.NotEmpty(data);
        }

        [Fact]
        public void ConvertCollection()
        {
            var input = new ViewEngine.FindViews.Arguments(new object[] {new ControllerContext(), "ViewName", false}, true);
            var output = new ViewEngineResult(Enumerable.Empty<string>());
            var timing = new TimerResult();
            var baseType = typeof (string);
            var isPartial = false;
            var id = Guid.NewGuid();
            var findViewsMessage1 = new ViewEngine.FindViews.Message(input, output, timing, baseType, isPartial, id);

            var input2 = new ViewEngine.FindViews.Arguments(new object[] { new ControllerContext(), "ViewName", false }, true);
            var view = new Mock<IView>().Object;
            var viewEngine = new Mock<IViewEngine>().Object;
            var output2 = new ViewEngineResult(view, viewEngine);
            var timing2 = new TimerResult();
            var baseType3 = typeof(string);
            var isPartial2 = false;
            var id2 = Guid.NewGuid();
            var findViewsMessage2 = new ViewEngine.FindViews.Message(input2, output2, timing2, baseType3, isPartial2, id2);

            var input3 = new ViewEngine.FindViews.Arguments(new object[] { new ControllerContext(), "ViewName", true }, true);
            var output3 = new ViewEngineResult(Enumerable.Empty<string>());
            var timing3 = new TimerResult();
            var baseType4 = typeof(string);
            var isPartial3 = false;
            var id3 = Guid.NewGuid();
            var findViewsMessage3 = new ViewEngine.FindViews.Message(input3, output3, timing3, baseType4, isPartial3, id3);

            var viewContext = new ViewContext {ViewData = new ViewDataDictionary(), TempData = new TempDataDictionary()};
            var textWriter = new StringWriter();
            var arguments = new View.Render.Arguments(new object[] { viewContext, textWriter });

            var timerResult = new TimerResult();
            var baseType2 = typeof (ViewRenderMessageShould);

            var mixinMock = new Mock<IViewCorrelation>();
            var mixin = mixinMock.Object;

            var renderMessage = new View.Render.Message(arguments, timerResult, baseType2, mixin);

            var vm1 = new ViewsModel(findViewsMessage1, renderMessage);
            var vm2 = new ViewsModel(findViewsMessage2, renderMessage);
            var vm3 = new ViewsModel(findViewsMessage3, renderMessage);

            var vms = new List<ViewsModel> {vm1, vm2, vm3};

            var converter = new ListOfViewsModelConverter();
            var result = converter.Convert(vms);

            var data = result as List<IEnumerable<object>>;

            Assert.NotNull(data);
            Assert.NotEmpty(data);
        }
    }
}