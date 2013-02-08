using System;
using System.IO;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Model;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Model
{
    public class ViewsModelShould
    {
        [Theory, AutoMock]
        public void SetProperties(ViewEngine.FindViews.Message findViewMessage, View.Render.Message viewRenderMessage)
        { 
            var model = new ViewsModel(findViewMessage, viewRenderMessage);

            Assert.Equal(findViewMessage.ViewName, model.ViewName);
            Assert.Equal(findViewMessage.MasterName, model.MasterName);
            Assert.Equal(findViewMessage.IsPartial, model.IsPartial);
            Assert.Equal(findViewMessage.BaseType, model.ViewEngineType);
            Assert.Equal(findViewMessage.UseCache, model.UseCache);
            Assert.Equal(findViewMessage.IsFound, model.IsFound);
            Assert.Equal(findViewMessage.SearchedLocations, model.SearchedLocations);
            Assert.NotNull(model.ViewModelSummary); 
        } 
    }
}