using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Common;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Framework
{
    public class CastleDynamicProxyFactoryShould
    {
        [Theory, AutoMock]
        public void ImplementIWrapper(CastleDynamicProxyFactory sut, IDisposable instance, IEnumerable<IAlternateMethod> methodInvocations)
        {
            var result = sut.WrapInterface(instance, methodInvocations);

            var resultAsWrapper = result as IWrapper<IDisposable>;

            Assert.NotNull(resultAsWrapper);
            Assert.Equal(instance, resultAsWrapper.GetWrappedObject());
        }

        [Theory, AutoMock]
        public void ExtendClassDoesNotImplementIWrapper(CastleDynamicProxyFactory sut, IEnumerable<IAlternateMethod> methodInvocations)
        {
            var result = sut.ExtendClass<TestProxy>(methodInvocations);

            var resultAsWrapper = result as IWrapper<TestProxy>;

            Assert.Null(resultAsWrapper);
        }

        [Theory, AutoMock]
        public void WrapClassDoesImplementIWrapper(CastleDynamicProxyFactory sut, IEnumerable<IAlternateMethod> methodInvocations)
        {
            var result = sut.WrapClass(new TestProxy(), methodInvocations);

            var resultAsWrapper = result as IWrapper<TestProxy>;

            Assert.NotNull(resultAsWrapper);
        }

        [Theory, AutoMock]
        public void WrapInterfaceDoesImplementIWrapper(CastleDynamicProxyFactory sut, IEnumerable<IAlternateMethod> methodInvocations)
        {
            var result = sut.WrapInterface<ITestProxy>(new TestProxy(), methodInvocations);

            var resultAsWrapper = result as IWrapper<ITestProxy>;

            Assert.NotNull(resultAsWrapper);
        }

        [Theory, AutoMock]
        public void IsWrapInterfaceEligibleOnlyAcceptsInterfaces(CastleDynamicProxyFactory sut)
        {
            var result = sut.IsWrapInterfaceEligible<ITestProxy>(typeof(ITestProxy)); 
            Assert.True(result);

            result = sut.IsWrapInterfaceEligible<ITestProxy>(typeof(TestProxy));
            Assert.False(result);
        }

        [Theory, AutoMock]
        public void IsWrapClassEligibleAcceptsNonSealedClasses(CastleDynamicProxyFactory sut)
        {
            var result = sut.IsWrapClassEligible(typeof(TestProxy)); 
            Assert.True(result);

            result = sut.IsWrapClassEligible(typeof(SealedTestProxy));
            Assert.False(result);
        }

        [Theory, AutoMock]
        public void IsExtendClassEligibleAcceptsNonSealedClasses(CastleDynamicProxyFactory sut)
        {
            var result = sut.IsExtendClassEligible(typeof(TestProxy));
            Assert.True(result);

            result = sut.IsExtendClassEligible(typeof(SealedTestProxy));
            Assert.False(result);
        }

        [Theory, AutoMock]
        public void IsExtendClassEligibleAcceptsOnlyClasses(CastleDynamicProxyFactory sut)
        {
            var result = sut.IsExtendClassEligible(typeof(TestProxy));
            Assert.True(result);

            result = sut.IsExtendClassEligible(typeof(ITestProxy));
            Assert.False(result);
        }

        [Fact]
        public void ThrowsWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new CastleDynamicProxyFactory(null, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On));
        }

        [Fact]
        public void ThrowsWithNullMessageBroker()
        {
            Assert.Throws<ArgumentNullException>(() => new CastleDynamicProxyFactory(new Mock<ILogger>().Object, null, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On));
        }

        [Fact]
        public void ThrowsWithNullTimerStrategy()
        {
            Assert.Throws<ArgumentNullException>(() => new CastleDynamicProxyFactory(new Mock<ILogger>().Object, new Mock<IMessageBroker>().Object, null, () => RuntimePolicy.On));
        }

        [Fact]
        public void ThrowsWithNullRuntimePolicyStrategy()
        {
            Assert.Throws<ArgumentNullException>(() => new CastleDynamicProxyFactory(new Mock<ILogger>().Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), null));
        }

        [Fact]
        public void ConstructWithLogger()
        {
            var loggerMock = new Mock<ILogger>();

            var factory = new CastleDynamicProxyFactory(loggerMock.Object, new Mock<IMessageBroker>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(loggerMock.Object, factory.Logger);
            Assert.NotNull(factory.ProxyGenerator);
        }

        [Theory(Skip = "Doesn't work because Moq and Glimpse both use Castle."), AutoMock]
        public void ReturnFalseForAlreadyProxiedObjectsOnIsProxyable(CastleDynamicProxyFactory sut, List<IAlternateMethod> methodInvocations)
        {
            var result = sut.ExtendClass<TestProxy>(methodInvocations);

            Assert.False(sut.IsExtendClassEligible(result.GetType()));
            Assert.False(sut.IsWrapClassEligible(result.GetType()));
        }

        [Theory, AutoMock]
        public void ExtendClassCreatesTrueProxy(ILogger logger, IMessageBroker messageBroker)
        {
            var timer = new ExecutionTimer(Stopwatch.StartNew());

            var sut = new CastleDynamicProxyFactory(logger, messageBroker, () => timer, () => RuntimePolicy.On);

            var overrideMeAlternate = new OverrideMeAlternateMethod<TestProxy>();
            var protectedOverrideMeAlternate = new ProtectedOverrideMeAlternateMethod<TestProxy>();

            var methodInvocations = new List<IAlternateMethod> { overrideMeAlternate, protectedOverrideMeAlternate };

            var result = sut.ExtendClass<TestProxy>(methodInvocations);
            result.OverrideMe();

            Assert.Equal(1, overrideMeAlternate.HitCount);
            Assert.Equal(1, protectedOverrideMeAlternate.HitCount);
            Assert.Equal(1, result.HitCountOverrideMe);
            Assert.Equal(1, result.HitCountProtectedOverrideMe);
        }

        [Theory, AutoMock]
        public void WrapClassCreatesWrappingProxy(ILogger logger, IMessageBroker messageBroker)
        {
            var timer = new ExecutionTimer(Stopwatch.StartNew());

            var sut = new CastleDynamicProxyFactory(logger, messageBroker, () => timer, () => RuntimePolicy.On);

            var overrideMeAlternate = new OverrideMeAlternateMethod<TestProxy>();
            var protectedOverrideMeAlternate = new ProtectedOverrideMeAlternateMethod<TestProxy>();

            var methodInvocations = new List<IAlternateMethod> { overrideMeAlternate, protectedOverrideMeAlternate };

            var target = new TestProxy();
            var result = sut.WrapClass(target, methodInvocations);
            result.OverrideMe();

            Assert.Equal(1, overrideMeAlternate.HitCount);
            Assert.Equal(0, protectedOverrideMeAlternate.HitCount);
            Assert.Equal(0, result.HitCountOverrideMe);
            Assert.Equal(0, result.HitCountProtectedOverrideMe);
            Assert.Equal(1, target.HitCountOverrideMe);
            Assert.Equal(1, target.HitCountProtectedOverrideMe);
        }

        [Theory, AutoMock]
        public void WrapInterfaceCreatesWrappingProxy(ILogger logger, IMessageBroker messageBroker)
        {
            var timer = new ExecutionTimer(Stopwatch.StartNew());

            var sut = new CastleDynamicProxyFactory(logger, messageBroker, () => timer, () => RuntimePolicy.On);

            var overrideMeAlternate = new OverrideMeAlternateMethod<ITestProxy>(); 

            var methodInvocations = new List<IAlternateMethod> { overrideMeAlternate };

            var target = new TestProxy();
            var result = sut.WrapInterface<ITestProxy>(target, methodInvocations);
            result.OverrideMe();

            Assert.Equal(1, overrideMeAlternate.HitCount); 
            Assert.Equal(1, result.HitCountOverrideMe);
            Assert.Equal(1, result.HitCountProtectedOverrideMe);
            Assert.Equal(1, target.HitCountOverrideMe);
            Assert.Equal(1, target.HitCountProtectedOverrideMe);
        }

        [Theory, AutoMock]
        public void SupportMixins(CastleDynamicProxyFactory factory)
        {
            const string expectedName = "any string"; 
            var dummyMixin = new List<object> { new DummyMixin { Name = expectedName } };

            var proxy = factory.ExtendClass<DummyTab>(Enumerable.Empty<IAlternateMethod>(), dummyMixin);

            Assert.NotNull(proxy);
            Assert.NotNull(proxy as ITab);

            var mixin = proxy as IDummyMixin;

            Assert.NotNull(mixin);
            Assert.Equal(expectedName, mixin.Name);
        }

        public interface ITestProxy
        {
            int HitCountOverrideMe { get; set; }

            int HitCountProtectedOverrideMe { get; set; }

            void OverrideMe();
        }

        public class TestProxy : ITestProxy
        {
            public int HitCountOverrideMe { get; set; }
            public int HitCountProtectedOverrideMe { get; set; }

            public virtual void OverrideMe()
            {
                HitCountOverrideMe++;
                ProtectedOverrideMe();
            }

            protected virtual void ProtectedOverrideMe()
            {
                HitCountProtectedOverrideMe++;
            }
        }

        public sealed class SealedTestProxy : ITestProxy
        {
            public int HitCountOverrideMe { get; set; }

            public int HitCountProtectedOverrideMe { get; set; }

            public void OverrideMe()
            {
            }
        }

        public class OverrideMeAlternateMethod<T> : AlternateMethod
        {
            public OverrideMeAlternateMethod()
                : base(typeof(T), "OverrideMe", BindingFlags.Public | BindingFlags.Instance)
            {
            }

            public int HitCount { get; set; }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                HitCount++;
            }
        }

        public class ProtectedOverrideMeAlternateMethod<T> : AlternateMethod
        {
            public ProtectedOverrideMeAlternateMethod()
                : base(typeof(T), "ProtectedOverrideMe", BindingFlags.NonPublic | BindingFlags.Instance)
            {
            }

            public int HitCount { get; set; }

            public override void PostImplementation(IAlternateImplementationContext context, TimerResult timerResult)
            {
                HitCount++;
            }
        }
    }
}