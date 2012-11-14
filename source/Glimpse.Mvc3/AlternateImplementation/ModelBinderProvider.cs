using System;
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
            yield return new GetBinder();
        }

        public class GetBinder : IAlternateImplementation<IModelBinderProvider>
        {
            public GetBinder()
            {
                MethodToImplement = typeof(IModelBinderProvider).GetMethod("GetBinder");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                var originalModelBinder = context.ReturnValue as IModelBinder;
            }
        }
    }
}