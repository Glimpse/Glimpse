using System;
using Glimpse.Mvc.AlternateImplementation;
using Xunit;

namespace Glimpse.Test.Mvc.AlternateImplementation
{
    public class ViewRenderMixinShould
    {
        [Fact]
        public void SetProperties()
        {
            var viewName = "AView";
            var isPartial = true;
            var id = Guid.NewGuid();

            var mixin = new View.Render.Mixin(viewName, isPartial, id);

            Assert.Equal(viewName, mixin.ViewName);
            Assert.Equal(isPartial, mixin.IsPartial);
            Assert.Equal(id, mixin.ViewEngineFindCallId);
        } 
    }
}