using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ValueProviderFactory : Alternate<System.Web.Mvc.ValueProviderFactory>
    {
        public ValueProviderFactory(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateImplementation<System.Web.Mvc.ValueProviderFactory>> AllMethods()
        {
            yield return new GetValueProvider(new ValueProvider<IValueProvider>(ProxyFactory), new ValueProvider<IUnvalidatedValueProvider>(ProxyFactory));
        }

        public class GetValueProvider : IAlternateImplementation<System.Web.Mvc.ValueProviderFactory>
        {
            public GetValueProvider(Alternate<IValueProvider> alternateValueProvider, Alternate<IUnvalidatedValueProvider> alternateUnvalidatedValueProvider)
            {
                AlternateValidatedValueProvider = alternateValueProvider;
                AlternateUnvalidatedValueProvider = alternateUnvalidatedValueProvider;
                MethodToImplement = typeof(System.Web.Mvc.ValueProviderFactory).GetMethod("GetValueProvider");
            }

            public MethodInfo MethodToImplement { get; private set; }

            public Alternate<IValueProvider> AlternateValidatedValueProvider { get; set; }

            public Alternate<IUnvalidatedValueProvider> AlternateUnvalidatedValueProvider { get; set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                var originalValueProvider = context.ReturnValue as IValueProvider;
                if (originalValueProvider == null)
                {
                    return;
                }

                var originalUnvalidatedValueProvider = originalValueProvider as IUnvalidatedValueProvider;
                if (originalUnvalidatedValueProvider != null)
                {
                    IUnvalidatedValueProvider newUnvalidatedValueProvider;
                    if (AlternateUnvalidatedValueProvider.TryCreate(originalUnvalidatedValueProvider, out newUnvalidatedValueProvider))
                    {
                        context.ReturnValue = newUnvalidatedValueProvider;
                        return;
                    }
                }

                IValueProvider newValueProvider;
                if (AlternateValidatedValueProvider.TryCreate(originalValueProvider, out newValueProvider))
                {
                    context.ReturnValue = newValueProvider;
                }
            }
        }
    }
}