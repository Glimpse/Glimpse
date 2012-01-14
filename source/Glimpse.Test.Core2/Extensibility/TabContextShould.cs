using System;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2.Extensibility
{
    public class TabContextShould:IDisposable
    {

        public TabContextShould()
        {
            RequestContext = new DummyObjectContext();
            PluginStoreMock = new Mock<IDataStore>();
            PipelineInspectors = new DiscoverableCollection<IPipelineInspector>
                                     {
                                         new DummyPipelineInspector1()
                                     };
        }



        private DiscoverableCollection<IPipelineInspector> PipelineInspectors { get; set; }

        private Mock<IDataStore> PluginStoreMock { get; set; }

        private DummyObjectContext RequestContext { get; set; }

        private TabContext tabContext;
        private TabContext TabContext
        {
            get { return tabContext ?? (tabContext = new TabContext(RequestContext, PluginStoreMock.Object, PipelineInspectors)); }
            set { tabContext = value; }
        }




        [Fact]
        public void Construct()
        {
            var pluginStoreObj = PluginStoreMock.Object;
            var locator = new TabContext(RequestContext, pluginStoreObj, PipelineInspectors);

            Assert.Equal(RequestContext, locator.GetRequestContext<DummyObjectContext>());
            Assert.Equal(pluginStoreObj, locator.PluginStore);
        }

        [Fact]
        public void GetPipelineModifier()
        {
            var inspector = TabContext.GetPipelineInspector<DummyPipelineInspector1>();
            Assert.NotNull(inspector);
            Assert.IsType<DummyPipelineInspector1>(inspector);
        }

        [Fact]
        public void ReturnNullWithMissingPipelineModifier()
        {
            var inspector = TabContext.GetPipelineInspector<DummyPipelineInspector2>();
            Assert.Null(inspector);
        }

        [Fact]
        public void ThrowWithNullPipelineInspectors()
        {
            Assert.Throws<ArgumentNullException>(() => new TabContext(RequestContext, PluginStoreMock.Object, null));
        }

        [Fact]
        public void ThrowWithNullPluginStore()
        {
            Assert.Throws<ArgumentNullException>(() => new TabContext(RequestContext, null, PipelineInspectors));
        }

        [Fact]
        public void ThrowWithNullRequestContext()
        {
            Assert.Throws<ArgumentNullException>(()=>new TabContext(null, PluginStoreMock.Object, PipelineInspectors));
        }

        public void Dispose()
        {
            RequestContext = null;
            PluginStoreMock = null;
            TabContext = null;
            PipelineInspectors = null;
        }
    }
}