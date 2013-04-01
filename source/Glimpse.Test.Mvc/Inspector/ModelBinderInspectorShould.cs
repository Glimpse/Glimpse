using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Inspector;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc.TestDoubles;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.Inspector
{
    public class ModelBinderInspectorShould : IDisposable
    {
        public void Dispose()
        {
            ModelBinders.Binders.DefaultBinder = new DefaultModelBinder();
        }

        [Fact]
        public void Constuct()
        {
            var sut = new ModelBinderInspector();

            Assert.IsAssignableFrom<IInspector>(sut);
        }

        [Theory, AutoMock]
        public void IgnoreEmptyModelBindingProvidersCollection(ModelBinderInspector sut, IInspectorContext context, IModelBinderProvider proxy)
        {
            ModelBinderProviders.BinderProviders.Clear();
            context.ProxyFactory.Setup(pf => pf.IsWrapInterfaceEligible<IModelBinderProvider>(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.WrapInterface(It.IsAny<IModelBinderProvider>(), It.IsAny<IEnumerable<IAlternateMethod>>(), Enumerable.Empty<object>())).Returns(proxy);

            sut.Setup(context);

            Assert.Empty(ModelBinderProviders.BinderProviders);
        }

        [Theory, AutoMock]
        public void UpdateModelBindingProviders(ModelBinderInspector sut, IInspectorContext context, IModelBinderProvider proxy)
        {
            ModelBinderProviders.BinderProviders.Add(new DummyModelBinderProvider());
            context.ProxyFactory.Setup(pf => pf.IsWrapInterfaceEligible<IModelBinderProvider>(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.WrapInterface(It.IsAny<IModelBinderProvider>(), It.IsAny<IEnumerable<IAlternateMethod>>(), Enumerable.Empty<object>())).Returns(proxy);

            sut.Setup(context);

            Assert.Contains(proxy, ModelBinderProviders.BinderProviders);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Theory, AutoMock]
        public void IgnoreEmptyValueProviderFactoriesCollection(ModelBinderInspector sut, IInspectorContext context, ValueProviderFactory proxy)
        {
            ValueProviderFactories.Factories.Clear();
            context.ProxyFactory.Setup(pf => pf.IsWrapClassEligible(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.WrapClass(It.IsAny<ValueProviderFactory>(), It.IsAny<IEnumerable<IAlternateMethod>>(), Enumerable.Empty<object>(), null)).Returns(proxy);

            sut.Setup(context);

            Assert.Empty(ValueProviderFactories.Factories);
        }

        [Theory, AutoMock]
        public void UpdateValueProviderFactories(ModelBinderInspector sut, IInspectorContext context, ValueProviderFactory proxy)
        {
            ValueProviderFactories.Factories.Add(new DummyValueProviderFactory());
            context.ProxyFactory.Setup(pf => pf.IsWrapClassEligible(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.WrapClass(It.IsAny<ValueProviderFactory>(), It.IsAny<IEnumerable<IAlternateMethod>>(), Enumerable.Empty<object>(), null)).Returns(proxy);

            sut.Setup(context);

            Assert.Contains(proxy, ValueProviderFactories.Factories);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Theory, AutoMock]
        public void UpdateModelBinders(ModelBinderInspector sut, IInspectorContext context, DummyDefaultModelBinder seedBinder, IModelBinder proxy)
        {
            ModelBinders.Binders.Add(typeof(object), seedBinder);
            context.ProxyFactory.Setup(pf => pf.IsWrapInterfaceEligible<IModelBinder>(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.WrapInterface(It.IsAny<IModelBinder>(), It.IsAny<IEnumerable<IAlternateMethod>>())).Returns(proxy);

            sut.Setup(context);

            Assert.Contains(proxy, ModelBinders.Binders.Values);
            Assert.DoesNotContain(seedBinder, ModelBinders.Binders.Values);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }

        [Theory, AutoMock]
        public void UpdateDefaultModelBinder(ModelBinderInspector sut, IInspectorContext context, DefaultModelBinder proxy)
        {
            context.ProxyFactory.Setup(pf => pf.IsExtendClassEligible(It.IsAny<Type>())).Returns(true);
            context.ProxyFactory.Setup(pf => pf.ExtendClass<DefaultModelBinder>(It.IsAny<IEnumerable<IAlternateMethod>>())).Returns(proxy);

            sut.Setup(context);

            Assert.Equal(proxy, ModelBinders.Binders.DefaultBinder);
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
        }
    }
}