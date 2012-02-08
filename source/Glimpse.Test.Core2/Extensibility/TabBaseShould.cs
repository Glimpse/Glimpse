using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class TabBaseShould
    {
        [Fact]
        public void DefaultToNullLifeCycleSupportNonGeneric()
        {
            var tab = new TestingTab();

            Assert.Null(tab.RequestContextType);
        }

        [Fact]
        public void DefaultLifecycleSupportToEndRequestNonGeneric()
        {
            var tab = new TestingTab();

            Assert.Equal(LifeCycleSupport.EndRequest, tab.LifeCycleSupport);
        }

        [Fact]
        public void DefaultToNullLifeCycleSupportGeneric()
        {
            var tab = new GenericTab();

            Assert.Equal(typeof(DummyObjectContext), tab.RequestContextType);
        }

        [Fact]
        public void DefaultLifecycleSupportToEndRequestGeneric()
        {
            var tab = new GenericTab();

            Assert.Equal(LifeCycleSupport.EndRequest, tab.LifeCycleSupport);
        }

        private class GenericTab : TabBase<DummyObjectContext>
        {
            public override object GetData(ITabContext context)
            {
                throw new System.NotImplementedException();
            }

            public override string Name
            {
                get { throw new System.NotImplementedException(); }
            }
        }

        private class TestingTab : TabBase
        {
            public override object GetData(ITabContext context)
            {
                throw new System.NotImplementedException();
            }

            public override string Name
            {
                get { throw new System.NotImplementedException(); }
            }
        }
    }
}