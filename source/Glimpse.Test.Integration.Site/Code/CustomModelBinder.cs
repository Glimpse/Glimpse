using System;
using System.Web.Mvc;
using Glimpse.Test.Integration.Site.Models;

namespace Glimpse.Test.Integration.Site.Code
{
    public class CustomModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = bindingContext.ValueProvider.GetValue("id");

            if (result == null)
            {
                return null;
            }

            return new CustomModel(Guid.Parse(result.AttemptedValue));
        }
    }
}