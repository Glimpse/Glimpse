using System;
using System.Web;
using System.Web.Mvc;
using Glimpse.Mvc3.Extensions;
using Glimpse.Mvc3.Warning;
using Glimpse.Net.Warning;
using Glimpse.WebForms.Extensions;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseBinderProvider:IModelBinderProvider
    {
        public IModelBinderProvider BinderProvider { get; set; }

        public GlimpseBinderProvider(IModelBinderProvider binderProvider)
        {
            BinderProvider = binderProvider;
        }

        public IModelBinder GetBinder(Type modelType)
        {
            var binder = BinderProvider.GetBinder(modelType);

            if (binder == null) return binder;

            if (binder is DefaultModelBinder)
            {
                if (binder.CanSupportDynamicProxy())
                    return binder.CreateDynamicProxy();
            }

            var warnings = HttpContext.Current.GetWarnings();
            warnings.Add(new NotADefaultModelBinderWarning(binder));
            
            return binder.Wrap();
        }
    }
}
