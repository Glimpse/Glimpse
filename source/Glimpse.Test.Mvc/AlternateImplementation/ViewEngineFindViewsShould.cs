using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Mvc.Tester;
using Xunit;

namespace Glimpse.Test.Mvc.AlternateImplementation
{
    public class ViewEngineFindViewsShould:IDisposable
    {
        private ViewEngineFindViewsTester implementation;
        public ViewEngineFindViewsTester Implementation
        {
            get { return implementation ?? (implementation = ViewEngineFindViewsTester.Create()); }
            set { implementation = value; }
        }

        public void Dispose()
        {
            Implementation = null;
        }

        [Fact]
        public void ReturnAllMethodImplementationsWithStaticAll()
        {
            var implementations = ViewEngine.AllMethods(Implementation.MessageBrokerMock.Object, Implementation.ProxyFactoryMock.Object, Implementation.LoggerMock.Object, () => Implementation.ExecutionTimerMock.Object, ()=>RuntimePolicy.On);

            Assert.Equal(2, implementations.Count());
        }

        [Fact]
        public void Construct()
        {
            var implementation = new ViewEngine.FindViews(Implementation.MessageBrokerMock.Object, Implementation.ProxyFactoryMock.Object, Implementation.LoggerMock.Object, () => new ExecutionTimer(Stopwatch.StartNew()), ()=>RuntimePolicy.On, false);

            Assert.NotNull(implementation);
            Assert.NotNull(implementation as IAlternateImplementation<IViewEngine>);
        }

        [Fact]
        public void MethodToImplementIsRight()
        {
            Assert.Equal(typeof(IViewEngine), Implementation.MethodToImplement.DeclaringType);
        }
    }
}