using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Test.Core.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class TabContextShould:IDisposable
    {

        public TabContextShould()
        {
            RequestContext = new DummyObjectContext();
            PluginStoreMock = new Mock<IDataStore>();
            LoggerMock = new Mock<ILogger>();
            MessageBroker = new Mock<IMessageBroker>();
            Inspectors = new ReflectionDiscoverableCollection<IInspector>(LoggerMock.Object)
                                     {
                                         new DummyInspector1()
                                     };
        }



        private IDiscoverableCollection<IInspector> Inspectors { get; set; }

        private Mock<IDataStore> PluginStoreMock { get; set; }

        private Mock<ILogger> LoggerMock { get; set; }

        private Mock<IMessageBroker> MessageBroker { get; set; }

        private DummyObjectContext RequestContext { get; set; }

        private TabContext tabContext;
        private TabContext TabContext
        {
            get { return tabContext ?? (tabContext = new TabContext(RequestContext, PluginStoreMock.Object, LoggerMock.Object, MessageBroker.Object)); }
            set { tabContext = value; }
        }




        [Fact]
        public void Construct()
        {
            var pluginStoreObj = PluginStoreMock.Object;
            var locator = new TabContext(RequestContext, pluginStoreObj, LoggerMock.Object, MessageBroker.Object);

            Assert.Equal(RequestContext, locator.GetRequestContext<DummyObjectContext>());
            Assert.Equal(pluginStoreObj, locator.TabStore);
        }

        [Fact]
        public void ThrowWithNullMessageBroker()
        {
            Assert.Throws<ArgumentNullException>(() => new TabContext(RequestContext, PluginStoreMock.Object, LoggerMock.Object, null));
        }

        [Fact]
        public void ThrowWithNullPluginStore()
        {
            Assert.Throws<ArgumentNullException>(() => new TabContext(RequestContext, null, LoggerMock.Object, MessageBroker.Object));
        }

        [Fact]
        public void ThrowWithNullRequestContext()
        {
            Assert.Throws<ArgumentNullException>(()=>new TabContext(null, PluginStoreMock.Object, LoggerMock.Object, MessageBroker.Object));
        }

        [Fact]
        public void ThrowWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new TabContext(RequestContext, PluginStoreMock.Object, null, MessageBroker.Object));
        }

        public void Dispose()
        {
            RequestContext = null;
            PluginStoreMock = null;
            TabContext = null;
            Inspectors = null;
            LoggerMock = null;
        }
    }
}