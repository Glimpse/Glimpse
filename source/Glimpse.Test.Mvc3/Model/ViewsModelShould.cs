using System;
using System.IO;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;
using Glimpse.Test.Mvc3.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.Model
{
    public class ViewsModelShould
    {
        [Fact]
        public void SetProperties()
        {
            var viewMock = new Mock<IView>();
            var viewEngineMock = new Mock<IViewEngine>();
            var viewName = "AName";
            var masterName = "AMaster";
            var input = new ViewEngine.FindViews.Arguments(new object[] { new ControllerContext(), viewName, masterName, false }, false);
            var output = new ViewEngineResult(viewMock.Object, viewEngineMock.Object);
            var timing = new TimerResult();
            var baseType1 = typeof(string);
            var isPartial = false;
            var id = Guid.NewGuid();

            var findViewMessage = new ViewEngine.FindViews.Message(input, output, timing, baseType1, isPartial, id);

            var viewContext = new ViewContext {ViewData = new ViewDataDictionary(), TempData = new TempDataDictionary()};

            var textWriter = new StringWriter();
            var arguments = new View.Render.Arguments(new object[] { viewContext, textWriter });

            var timerResult = new TimerResult();
            var baseType2 = typeof(ViewRenderMessageShould);

            var mixinMock = new Mock<View.Render.IMixin>();
            var mixin = mixinMock.Object;

            var viewRenderMessage = new View.Render.Message(arguments, timerResult, baseType2, mixin);

            var model = new ViewsModel(findViewMessage, viewRenderMessage);

            Assert.Equal(viewName, model.ViewName);
            Assert.Equal(masterName, model.MasterName);
            Assert.Equal(isPartial, model.IsPartial);
            Assert.Equal(baseType1, model.ViewEngineType);
            Assert.False(model.UseCache);
            Assert.True(model.IsFound);
            Assert.Null(model.SearchedLocations);
            Assert.NotNull(model.ViewModelSummary);
        } 
    }
}