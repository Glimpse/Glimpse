using System;
using System.IO;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.AlternateImplementation;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Model
{
    public class ViewsModelShould
    {
        [Theory, AutoMock]
        public void SetProperties(ViewEngine.FindViews.Arguments findViewArgs, View.Render.Arguments renderArgs, ViewEngineResult viewEngineResult, IViewCorrelationMixin mixin, TimerResult timerResult, Type type)
        {
            var findViewMessage = new ViewEngine.FindViews.Message(findViewArgs, timerResult, typeof(IViewEngine), null, viewEngineResult, type, false, Guid.NewGuid());

            var viewRenderMessage = new View.Render.Message(renderArgs, timerResult, type, mixin);

            var model = new ViewsModel(findViewMessage, viewRenderMessage);

            Assert.Equal(findViewArgs.ViewName, model.ViewName);
            Assert.Equal(findViewArgs.MasterName, model.MasterName);
            Assert.False(model.IsPartial);
            Assert.Equal(type, model.ViewEngineType);
            Assert.True(model.UseCache);
            Assert.True(model.IsFound);
            Assert.Null(model.SearchedLocations);
            Assert.NotNull(model.ViewModelSummary);
        } 
    }
}