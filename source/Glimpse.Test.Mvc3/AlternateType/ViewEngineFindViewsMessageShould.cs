using System;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ViewEngineFindViewsMessageShould
    {
        [Theory, AutoMock]
        public void NotFindViewWithMissingViewEngineResult(ViewEngine.FindViews.Arguments input, TimerResult timing, Type baseType, bool isPartial, Guid id)
        {
            var output = new ViewEngineResult(Enumerable.Empty<string>());

            var sut = new ViewEngine.FindViews.Message(input, timing, typeof(IViewEngine), null, output, baseType, isPartial, id);

            Assert.Equal(input.MasterName, sut.MasterName);
            Assert.Equal(input.ViewName, sut.ViewName);
            Assert.Equal(input.UseCache, sut.UseCache);
            Assert.Equal(output.SearchedLocations, sut.SearchedLocations);
            Assert.Equal(timing.Duration, sut.Duration);
            Assert.Equal(timing.StartTime, sut.StartTime);
            Assert.Equal(timing.Offset, sut.Offset);
            Assert.Equal(isPartial, sut.IsPartial);
            Assert.Equal(id, sut.Id);
            Assert.False(sut.IsFound);
        }

        [Theory, AutoMock]
        public void FindViewWithViewEngineResult(ViewEngine.FindViews.Arguments input, TimerResult timing, Type baseType, bool isPartial, Guid id)
        {
            var output = new ViewEngineResult(new Mock<IView>().Object, new Mock<IViewEngine>().Object);

            var sut = new ViewEngine.FindViews.Message(input, timing, typeof(IViewEngine), null, output, baseType, isPartial, id);

            Assert.Equal(input.MasterName, sut.MasterName);
            Assert.Equal(input.ViewName, sut.ViewName);
            Assert.Equal(input.UseCache, sut.UseCache);
            Assert.Equal(output.SearchedLocations, sut.SearchedLocations);
            Assert.Equal(timing.Duration, sut.Duration);
            Assert.Equal(timing.StartTime, sut.StartTime);
            Assert.Equal(timing.Offset, sut.Offset);
            Assert.Equal(isPartial, sut.IsPartial);
            Assert.Equal(id, sut.Id);
            Assert.True(sut.IsFound);
        } 
    }
}