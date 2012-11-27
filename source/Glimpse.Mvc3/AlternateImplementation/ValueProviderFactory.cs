using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using MvcValueProviderFactory = System.Web.Mvc.ValueProviderFactory;
#if MVC2
using Glimpse.Mvc2.Backport;
#endif

namespace Glimpse.Mvc.AlternateImplementation
{
    public class ValueProviderFactory : Alternate<MvcValueProviderFactory>
    {
        public ValueProviderFactory(IProxyFactory proxyFactory) : base(proxyFactory)
        {
            AlternateValidatedValueProvider = new ValueProvider<IValueProvider>(ProxyFactory);
            AlternateUnvalidatedValueProvider = new ValueProvider<IUnvalidatedValueProvider>(ProxyFactory);
        }

        private ValueProvider<IUnvalidatedValueProvider> AlternateUnvalidatedValueProvider { get; set; }

        private ValueProvider<IValueProvider> AlternateValidatedValueProvider { get; set; }

        public override IEnumerable<IAlternateImplementation<MvcValueProviderFactory>> AllMethods()
        {
            yield return new GetValueProvider(ProxyValueProviderStrategy);
        }

        public override bool TryCreate(MvcValueProviderFactory originalObj, out MvcValueProviderFactory newObj, object mixin)
        {
            if (!base.TryCreate(originalObj, out newObj, mixin))
            {
                newObj = new ValueProviderFactoryDecorator(originalObj, ProxyValueProviderStrategy);
            }

            return true;
        }

        private IValueProvider ProxyValueProviderStrategy(IValueProvider original)
        {
            if (original == null)
            {
                return null;
            }

            var originalUnvalidatedValueProvider = original as IUnvalidatedValueProvider;
            if (originalUnvalidatedValueProvider != null)
            {
                IUnvalidatedValueProvider newUnvalidatedValueProvider;
                if (AlternateUnvalidatedValueProvider.TryCreate(originalUnvalidatedValueProvider, out newUnvalidatedValueProvider))
                {
                    return newUnvalidatedValueProvider;
                }
            }

            IValueProvider newValueProvider;
            if (AlternateValidatedValueProvider.TryCreate(original, out newValueProvider))
            {
                return newValueProvider;
            }

            return null;
        }

        public class GetValueProvider : IAlternateImplementation<System.Web.Mvc.ValueProviderFactory>
        {
            public GetValueProvider(Func<IValueProvider, IValueProvider> proxyValueProviderStrategy)
            {
                MethodToImplement = typeof(System.Web.Mvc.ValueProviderFactory).GetMethod("GetValueProvider");
                ProxyValueProviderStrategy = proxyValueProviderStrategy;
            }

            public MethodInfo MethodToImplement { get; private set; }

            internal Func<IValueProvider, IValueProvider> ProxyValueProviderStrategy { get; set; }

            public void NewImplementation(IAlternateImplementationContext context)
            {
                TimerResult timerResult;
                if (!context.TryProceedWithTimer(out timerResult))
                {
                    return;
                }

                var originalValueProvider = context.ReturnValue as IValueProvider;

                var result = ProxyValueProviderStrategy(originalValueProvider);

                if (result != null)
                {
                    context.ReturnValue = result;
                }
            }
        }

        public class ValueProviderFactoryDecorator : MvcValueProviderFactory
        {
            public ValueProviderFactoryDecorator(MvcValueProviderFactory wrappedValueProviderFactory, Func<IValueProvider, IValueProvider> proxyValueProviderStrategy)
            {
                WrappedValueProviderFactory = wrappedValueProviderFactory;
                ProxyValueProviderStrategy = proxyValueProviderStrategy;
            }

            internal MvcValueProviderFactory WrappedValueProviderFactory { get; set; }

            internal Func<IValueProvider, IValueProvider> ProxyValueProviderStrategy { get; set; }

            public override IValueProvider GetValueProvider(ControllerContext controllerContext)
            {
                var result = WrappedValueProviderFactory.GetValueProvider(controllerContext);

                return ProxyValueProviderStrategy(result);
            }
        }
    }
}