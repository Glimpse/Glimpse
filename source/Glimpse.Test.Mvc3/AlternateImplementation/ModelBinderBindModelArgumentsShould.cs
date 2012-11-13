using System;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ModelBinderBindModelArgumentsShould
    {
        [Theory, AutoMock]
        public void Construct(ControllerContext controllerContext, ModelBindingContext modelBindingContext)
        {
            var sut = new ModelBinder.BindModel.Arguments(controllerContext, modelBindingContext);

            Assert.Equal(controllerContext, sut.ControllerContext);
            Assert.Equal(modelBindingContext, sut.ModelBindingContext);
        }

        [Theory, AutoMock]
        public void ThrowWithInvalidControllerContext(object controllerContext, ModelBindingContext modelBindingContext)
        {
            Assert.Throws<InvalidCastException>(() => new ModelBinder.BindModel.Arguments(controllerContext, modelBindingContext));
        }

        [Theory, AutoMock]
        public void ThrowWithInvalidModelBindingContext(ControllerContext controllerContext, object modelBindingContext)
        {
            Assert.Throws<InvalidCastException>(() => new ModelBinder.BindModel.Arguments(controllerContext, modelBindingContext));
        }
    }
}