using System;
using System.ComponentModel;
using System.Web.Mvc;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ModelBinderBindPropertyArgumentsShould
    {
        [Theory, AutoMock]
        public void Construct(ControllerContext controllerContext, ModelBindingContext modelBindingContext, PropertyDescriptor propertyDescriptor)
        {
            var sut = new ModelBinder.BindProperty.Arguments(controllerContext, modelBindingContext, propertyDescriptor);

            Assert.Equal(controllerContext, sut.ControllerContext);
            Assert.Equal(modelBindingContext, sut.ModelBindingContext);
            Assert.Equal(propertyDescriptor, sut.PropertyDescriptor);
        }

        [Theory, AutoMock]
        public void ThrowWithInvalidControllerContext(object controllerContext, ModelBindingContext modelBindingContext, PropertyDescriptor propertyDescriptor)
        {
            Assert.Throws<InvalidCastException>(() => new ModelBinder.BindProperty.Arguments(controllerContext, modelBindingContext, propertyDescriptor));
        }

        [Theory, AutoMock]
        public void ThrowWithInvalidModelBindingContext(ControllerContext controllerContext, object modelBindingContext, PropertyDescriptor propertyDescriptor)
        {
            Assert.Throws<InvalidCastException>(() => new ModelBinder.BindProperty.Arguments(controllerContext, modelBindingContext, propertyDescriptor));
        }

        [Theory, AutoMock]
        public void ThrowWithInvalidPropertyDescriptor(ControllerContext controllerContext, ModelBindingContext modelBindingContext, object propertyDescriptor)
        {
            Assert.Throws<InvalidCastException>(() => new ModelBinder.BindProperty.Arguments(controllerContext, modelBindingContext, propertyDescriptor));
        }
    }
}
