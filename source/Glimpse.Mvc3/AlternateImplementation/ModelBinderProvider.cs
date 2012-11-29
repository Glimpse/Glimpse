using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ModelBinderProvider : Alternate<IModelBinderProvider>
    {
        public ModelBinderProvider(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<IModelBinderProvider>> AllMethods()
        {
            yield return new GetBinder(new ModelBinder(ProxyFactory));
        }

        public class GetBinder : IAlternateImplementation<IModelBinderProvider>
        {
            public GetBinder(Alternate<DefaultModelBinder> alternateModelBinder)
            {
                MethodToImplement = typeof(IModelBinderProvider).GetMethod("GetBinder");
                AlternateModelBinder = alternateModelBinder;
            }

            public Alternate<DefaultModelBinder> AlternateModelBinder { get; set; }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                var originalModelBinder = context.ReturnValue as DefaultModelBinder;
                
                // Can only wrap implementations of DefaultModelBinder (not IModelBinder!) for now
                if (originalModelBinder == null)
                {
                    context.Logger.Warn(Resources.GetBinderNewImplementationCannotProxyWarning, context.ReturnValue.GetType());
                    return;
                }

                DefaultModelBinder newModelBinder;
                if (AlternateModelBinder.TryCreate(originalModelBinder, out newModelBinder))
                {
                    context.ReturnValue = newModelBinder;
                }
            }
        }
    }
}