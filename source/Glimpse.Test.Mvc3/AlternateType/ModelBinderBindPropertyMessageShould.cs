using System;
using System.Reflection;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ModelBinderBindPropertyMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ModelBinder.BindProperty.Arguments arguments, Type modelBinderType, MethodInfo executedMethod)
        {
            var sut = new ModelBinder.BindProperty.Message(arguments, modelBinderType, executedMethod);

            Assert.Equal(arguments.PropertyDescriptor.Name, sut.Name);
            Assert.Equal(arguments.PropertyDescriptor.PropertyType, sut.Type);
            Assert.Equal(modelBinderType, sut.ModelBinderType);
        }
    }
}