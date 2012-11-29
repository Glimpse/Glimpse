using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Castle.DynamicProxy;
using Xunit;

namespace Glimpse.Test.Core
{
    public interface ISpecialDisposable : IDisposable
    {
        string GetReasonToDispose();
    }

    public class SimpleSpecialDisposable : ISpecialDisposable
    {
        public void Dispose()
        {
        }

        public string GetReasonToDispose()
        {
            return "I'm open";
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This class is only used to setup inheritance chains only used in these tests.")]
    public class CrazyConstructorSpecialDisposable : ISpecialDisposable
    {
        public CrazyConstructorSpecialDisposable(string a, string b)
        {
            A = a;
            B = b;
        }

        public string B { get; set; }

        public string A { get; set; }

        public void Dispose()
        {
        }

        public string GetReasonToDispose()
        {
            return "I'm open";
        }
    }

    public class ReallyCrazyConstructorSpecialDisposable : CrazyConstructorSpecialDisposable
    {
        public ReallyCrazyConstructorSpecialDisposable(string a) : base(a, "derived-b")
        {
        }
    }

    public sealed class SealedSpecialDisposable : ISpecialDisposable
    {
        public void Dispose()
        {
        }

        public string GetReasonToDispose()
        {
            return "I'm sealed";
        }
    }

    // These tests are more of a sanity check/documentation of Castle than acctual tests.
    public class CastleShould
    {
        [Fact]
        public void CastleTestInterfaces()
        {
            var proxyGenerator = new ProxyGenerator();

            var originalObj = new SimpleSpecialDisposable();

            var newObj = proxyGenerator.CreateInterfaceProxyWithTarget(typeof(IDisposable), originalObj, Enumerable.Empty<IInterceptor>().ToArray());

            Assert.IsAssignableFrom<IDisposable>(newObj);
            Assert.Throws<InvalidCastException>(() => (ISpecialDisposable)newObj);
            Assert.Throws<InvalidCastException>(() => (SimpleSpecialDisposable)newObj);
        }
        
        [Fact]
        public void CastleTestClass1()
        {
            var proxyGenerator = new ProxyGenerator();

            var originalObj = new SimpleSpecialDisposable();

            var newObj = proxyGenerator.CreateClassProxyWithTarget(originalObj, Enumerable.Empty<IInterceptor>().ToArray());

            Assert.IsAssignableFrom<IDisposable>(newObj);
            Assert.IsAssignableFrom<ISpecialDisposable>(newObj);
            Assert.IsAssignableFrom<SimpleSpecialDisposable>(newObj);
        }

        [Fact]
        public void CastleTestSealedClass()
        {
            var proxyGenerator = new ProxyGenerator();

            var originalObj = new SealedSpecialDisposable();

            Assert.Throws<TypeLoadException>(() => proxyGenerator.CreateClassProxyWithTarget(originalObj, Enumerable.Empty<IInterceptor>().ToArray()));
        }

        [Fact]
        public void CastleTestConstructorClass()
        {
            var proxyGenerator = new ProxyGenerator();

            var originalObj = new CrazyConstructorSpecialDisposable("test", "test2");

            var newObj = (CrazyConstructorSpecialDisposable)proxyGenerator.CreateClassProxyWithTarget(originalObj.GetType(), originalObj, new object[] { "a", "b" }, Enumerable.Empty<IInterceptor>().ToArray());

            Assert.IsAssignableFrom<IDisposable>(newObj);
            Assert.IsAssignableFrom<ISpecialDisposable>(newObj);
            Assert.IsAssignableFrom<CrazyConstructorSpecialDisposable>(newObj);
            Assert.Equal("a", newObj.A);
            Assert.Equal("b", newObj.B);
        }

        [Fact]
        public void CastleTestReallyCrazyConstructorClass()
        {
            var proxyGenerator = new ProxyGenerator();

            var originalObj = new ReallyCrazyConstructorSpecialDisposable("3");

            Assert.Throws<InvalidProxyConstructorArgumentsException>(
                () => proxyGenerator.CreateClassProxyWithTarget(originalObj.GetType(), originalObj, new object[] { "a", "b" }, Enumerable.Empty<IInterceptor>().ToArray()));
        }
    }
}