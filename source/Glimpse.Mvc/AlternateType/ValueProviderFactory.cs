using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;

#if MVC2
using Glimpse.Mvc2.Backport;
#endif
#if MVC3
using Glimpse.Mvc3.Backport;
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
                        new GetValueProvider(
                            new ValueProvider<IValueProvider>(ProxyFactory), 
                            new ValueProvider<IUnvalidatedValueProvider>(ProxyFactory),
                            new ValueProvider<IEnumerableValueProvider>(ProxyFactory),
                            new ValueProvider<IUnvalidatedEnumerableValueProvider>(ProxyFactory))
                    });
            }
        }

        public class GetValueProvider : AlternateMethod
        {
            public GetValueProvider(
                ValueProvider<IValueProvider> alternateValidatedValueProvider,
                ValueProvider<IUnvalidatedValueProvider> alternateUnvalidatedValueProvider,
                ValueProvider<IEnumerableValueProvider> alternateValidatedEnumerableValueProvider,
                ValueProvider<IUnvalidatedEnumerableValueProvider> alternateUnvalidatedEnumerableValueProvider)
                : base(typeof(System.Web.Mvc.ValueProviderFactory), "GetValueProvider")
            {
                AlternateValidatedValueProvider = alternateValidatedValueProvider;
                AlternateUnvalidatedValueProvider = alternateUnvalidatedValueProvider;
                AlternateValidatedEnumerableValueProvider = alternateValidatedEnumerableValueProvider;
                AlternateUnvalidatedEnumerableValueProvider = alternateUnvalidatedEnumerableValueProvider;
            }

            private ValueProvider<IUnvalidatedValueProvider> AlternateUnvalidatedValueProvider { get; set; }

            private ValueProvider<IValueProvider> AlternateValidatedValueProvider { get; set; }

            private ValueProvider<IEnumerableValueProvider> AlternateValidatedEnumerableValueProvider { get; set; }

            private ValueProvider<IUnvalidatedEnumerableValueProvider> AlternateUnvalidatedEnumerableValueProvider { get; set; }

            public override void PostImplementation(IAlternateMethodContext context, TimerResult timerResult)
            {
                var originalUnvalidatedValueProvider = context.ReturnValue as IUnvalidatedValueProvider;
                var originalEnumerableValueProvider = context.ReturnValue as IEnumerableValueProvider;

                if (originalUnvalidatedValueProvider != null && originalEnumerableValueProvider != null)
                {
                    IUnvalidatedEnumerableValueProvider newUnvalidatedEnumerableValueProvider;
                    if (AlternateUnvalidatedEnumerableValueProvider.TryCreate(new UnvalidatedEnumerableValueProvider(context.ReturnValue), out newUnvalidatedEnumerableValueProvider))
                    {
                        context.ReturnValue = newUnvalidatedEnumerableValueProvider;
                        return;
                    }
                }
                else if (originalUnvalidatedValueProvider != null)
                {
                    IUnvalidatedValueProvider newUnvalidatedValueProvider;
                    if (AlternateUnvalidatedValueProvider.TryCreate(originalUnvalidatedValueProvider, out newUnvalidatedValueProvider))
                    {
                        context.ReturnValue = newUnvalidatedValueProvider;
                        return;
                    }
                }
                else if (originalEnumerableValueProvider != null)
                {
                    IEnumerableValueProvider newEnumerableValueProvider;
                    if (AlternateValidatedEnumerableValueProvider.TryCreate(originalEnumerableValueProvider, out newEnumerableValueProvider))
                    {
                        context.ReturnValue = newEnumerableValueProvider;
                        return;
                    }
                }

                var originalValueProvider = context.ReturnValue as IValueProvider;
                if (originalValueProvider != null)
                {
                    IValueProvider newValueProvider;
                    if (AlternateValidatedValueProvider.TryCreate(originalValueProvider, out newValueProvider))
                    {
                        context.ReturnValue = newValueProvider;
                    }
                }
            }
        }

        public interface IUnvalidatedEnumerableValueProvider : IEnumerableValueProvider, IUnvalidatedValueProvider
        {
        }

        private class UnvalidatedEnumerableValueProvider : IUnvalidatedEnumerableValueProvider
        {
            private IUnvalidatedValueProvider ValueProviderAsUnvalidatedValueProvider { get; set; }

            private IEnumerableValueProvider ValueProviderAsEnumerableValueProvider { get; set; }

            public UnvalidatedEnumerableValueProvider(object valueProvider)
            {
                this.ValueProviderAsUnvalidatedValueProvider = valueProvider as IUnvalidatedValueProvider;
                this.ValueProviderAsEnumerableValueProvider = valueProvider as IEnumerableValueProvider;

                if (this.ValueProviderAsUnvalidatedValueProvider == null)
                {
                    throw new ArgumentException("The valueProvider must be a '" + typeof(IUnvalidatedValueProvider).FullName + "'.");
                }

                if (this.ValueProviderAsEnumerableValueProvider == null)
                {
                    throw new ArgumentException("The valueProvider must be a '" + typeof(IEnumerableValueProvider).FullName + "'.");
                }
            }

            public bool ContainsPrefix(string prefix)
            {
                return this.ValueProviderAsUnvalidatedValueProvider.ContainsPrefix(prefix);
            }

            public ValueProviderResult GetValue(string key)
            {
                return this.ValueProviderAsUnvalidatedValueProvider.GetValue(key);
            }

            public ValueProviderResult GetValue(string key, bool skipValidation)
            {
                return this.ValueProviderAsUnvalidatedValueProvider.GetValue(key, skipValidation);
            }

            public IDictionary<string, string> GetKeysFromPrefix(string prefix)
            {
                return this.ValueProviderAsEnumerableValueProvider.GetKeysFromPrefix(prefix);
            }
        }
    }
}