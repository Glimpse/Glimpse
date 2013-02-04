using System;
using System.Reflection;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ModelBinderBindModelMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ModelBinder.BindModel.Arguments arguments, Type modelBinderType, object rawValue, MethodInfo method)
        {
            var sut = new ModelBinder.BindModel.Message(arguments, modelBinderType, rawValue, method);

            Assert.Equal(arguments.ModelBindingContext.ModelName, sut.ModelName);
            Assert.Equal(arguments.ModelBindingContext.ModelType, sut.ModelType);
            Assert.Equal(modelBinderType, sut.ModelBinderType);
            Assert.Equal(rawValue, sut.RawValue);
        }
    }
}