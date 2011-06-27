using System;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Plumbing;
using Glimpse.Mvc3.Extensions;

namespace Glimpse.Mvc3.Plumbing
{
    internal class GlimpseBinderProvider:IModelBinderProvider
    {
        public IModelBinderProvider BinderProvider { get; set; }
        public IGlimpseLogger Logger { get; set; }

        public GlimpseBinderProvider(IModelBinderProvider binderProvider, IGlimpseLogger logger)
        {
            BinderProvider = binderProvider;
            Logger = logger;
        }

        public IModelBinder GetBinder(Type modelType)
        {
            var binder = BinderProvider.GetBinder(modelType);

            if (binder == null) return binder;

            if (binder is DefaultModelBinder)
            {
                if (binder.CanSupportDynamicProxy(Logger))
                    return binder.CreateDynamicProxy();
            }

            Logger.Warn(binder.GetType() + " is not a System.Web.Mvc.DefaultModelBinder.");
            
            return binder.Wrap();
        }
    }
}
