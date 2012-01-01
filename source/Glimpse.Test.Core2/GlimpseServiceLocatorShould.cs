using System;
using Glimpse.Core2;
using Glimpse.Core2.Extensibility;
using Glimpse.Test.Core2.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class GlimpseServiceLocatorShould:IDisposable
    {

        public GlimpseServiceLocatorShould()
        {
            RequestContext = new DummyObjectContext();
            PluginStoreMock = new Mock<IDataStore>();
            PipelineInspectors = new GlimpseCollection<IGlimpsePipelineInspector>
                                     {
                                         new DummyGlimpsePipelineInspector1()
                                     };
        }



        private GlimpseCollection<IGlimpsePipelineInspector> PipelineInspectors { get; set; }

        private Mock<IDataStore> PluginStoreMock { get; set; }

        private DummyObjectContext RequestContext { get; set; }

        private GlimpseServiceLocator serviceLocator;
        private GlimpseServiceLocator ServiceLocator
        {
            get { return serviceLocator ?? (serviceLocator = new GlimpseServiceLocator(RequestContext, PluginStoreMock.Object, PipelineInspectors)); }
            set { serviceLocator = value; }
        }




        [Fact]
        public void Construct()
        {
            var pluginStoreObj = PluginStoreMock.Object;
            var locator = new GlimpseServiceLocator(RequestContext, pluginStoreObj, PipelineInspectors);

            Assert.Equal(RequestContext, locator.RequestContext);
            Assert.Equal(pluginStoreObj, locator.PluginStore);
        }

        [Fact]
        public void GetPipelineModifier()
        {
            var inspector = ServiceLocator.GetPipelineInspector<DummyGlimpsePipelineInspector1>();
            Assert.NotNull(inspector);
            Assert.IsType<DummyGlimpsePipelineInspector1>(inspector);
        }

        [Fact]
        public void ReturnNullWithMissingPipelineModifier()
        {
            var inspector = ServiceLocator.GetPipelineInspector<DummyPipelineInspector2>();
            Assert.Null(inspector);
        }

        [Fact]
        public void ThrowWithNullPipelineInspectors()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseServiceLocator(RequestContext, PluginStoreMock.Object, null));
        }

        [Fact]
        public void ThrowWithNullPluginStore()
        {
            Assert.Throws<ArgumentNullException>(() => new GlimpseServiceLocator(RequestContext, null, PipelineInspectors));
        }

        [Fact]
        public void ThrowWithNullRequestContext()
        {
            Assert.Throws<ArgumentNullException>(()=>new GlimpseServiceLocator(null, PluginStoreMock.Object, PipelineInspectors));
        }

        public void Dispose()
        {
            RequestContext = null;
            PluginStoreMock = null;
            ServiceLocator = null;
            PipelineInspectors = null;
        }
    }
}