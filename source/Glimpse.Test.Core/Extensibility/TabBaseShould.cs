using Glimpse.Core.Extensibility;
using Glimpse.Test.Core.TestDoubles;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
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

            Assert.Equal(RuntimeEvent.EndRequest, tab.ExecuteOn);
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

            Assert.Equal(RuntimeEvent.EndRequest, tab.ExecuteOn);
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