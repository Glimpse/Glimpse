using System;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewRenderMixinShould
    {
        [Theory, AutoMock]
        public void SetProperties(string viewName, bool isPartial, Guid id)
        {
            var sut = new ViewCorrelationMixin(viewName, isPartial, id);

            Assert.Equal(viewName, sut.ViewName);
            Assert.Equal(isPartial, sut.IsPartial);
            Assert.Equal(id, sut.ViewEngineFindCallId);
        } 
    }
}