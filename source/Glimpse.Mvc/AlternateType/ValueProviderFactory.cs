using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
#if MVC2
using Glimpse.Mvc2.Backport;
#endif

namespace Glimpse.Mvc.AlternateType
{
    public class ValueProviderFactory : AlternateType<System.Web.Mvc.ValueProviderFactory>
    {
        private IEnumerable<IAlternateMethod> allMethods;

        public ValueProviderFactory(IProxyFactory proxyFactory) : base(proxyFactory)
        {
        }

        public override IEnumerable<IAlternateMethod> AllMethods
        {
            get
            {
                return allMethods ?? (allMethods = new List<IAlternateMethod>
                    {
                        new GetValueProvider(new ValueProvider<IValueProvider>(ProxyFactory), new ValueProvider<IUnvalidatedValueProvider>(ProxyFactory))
                    });
            }
        }

        public class GetValueProvider : AlternateMethod
        {
            public GetValueProvider(ValueProvider<IValueProvider> alternateValidatedValueProvider, ValueProvider<IUnvalidatedValueProvider> alternateUnvalidatedValueProvider) : base(typeof(System.Web.Mvc.ValueProviderFactory), "GetValueProvider")
            {
                AlternateValidatedValueProvider = alternateValidatedValueProvider;
                AlternateUnvalidatedValueProvider = alternateUnvalidatedValueProvider;
            }

            private ValueProvider<IUnvalidatedValueProvider> AlternateUnvalidatedValueProvider { get; set; }

            private ValueProvider<IValueProvider> AlternateValidatedValueProvider { get; set; }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var original = context.ReturnValue as IValueProvider;

                if (original == null)
                {
                    return;
                }

                var originalUnvalidatedValueProvider = original as IUnvalidatedValueProvider;
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
                if (AlternateValidatedValueProvider.TryCreate(original, out newValueProvider))
                {
                    context.ReturnValue = newValueProvider;
                }
            }
        }
    }
}