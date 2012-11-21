using System;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ModelBinderBindPropertyMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ModelBinder.BindProperty.Arguments arguments, Type modelBinderType)
        {
            var sut = new ModelBinder.BindProperty.Message(arguments, modelBinderType);

            Assert.Equal(arguments.PropertyDescriptor.Name, sut.Name);
            Assert.Equal(arguments.PropertyDescriptor.PropertyType, sut.Type);
            Assert.Equal(modelBinderType, sut.ModelBinderType);
        }
    }
}