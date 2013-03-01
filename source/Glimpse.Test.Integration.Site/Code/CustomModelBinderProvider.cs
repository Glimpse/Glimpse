using System;
using System.Web.Mvc;
using Glimpse.Test.Integration.Site.Models;

namespace Glimpse.Test.Integration.Site.Code
{
    public class CustomModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(CustomModel))
                return new CustomModelBinder();

            return null;
        }
    }
}