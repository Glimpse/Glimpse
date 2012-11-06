using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.Tab;
using Glimpse.Test.Mvc3.AlternateImplementation;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.Tab
{
    public class MetadataShould
    {
        [Fact]
        public void Construct()
        {
            var metadata = new Metadata();

            Assert.NotNull(metadata as ITab);
            Assert.NotNull(metadata as ITabSetup);
        }

        [Fact]
        public void ExecuteOnEndRequest()
        {
            var metadata = new Metadata();

            Assert.Equal(RuntimeEvent.EndRequest, metadata.ExecuteOn);
        }

        [Fact]
        public void HaveHttpContextBase()
        {
            var metadata = new Metadata();
            Assert.Equal(typeof(HttpContextBase), metadata.RequestContextType);
        }

        [Fact]
        public void HaveProperName()
        {
            var metadata = new Metadata();
            Assert.Equal("Metadata", metadata.Name);

        }

        [Fact]
        public void SubscribeToViewMessageTypes()
        {
            var messageBrokerMock = new Mock<IMessageBroker>();
            var contextMock = new Mock<ITabSetupContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(messageBrokerMock.Object);

            var tab = new Metadata();
            tab.Setup(contextMock.Object);

            messageBrokerMock.Verify(mb => mb.Subscribe(It.IsAny<Action<View.Render.Message>>()));
        }

        [Fact]
        public void HandleNullFindViewMessageCollection()
        {
            var storeMock = new Mock<IDataStore>();
            storeMock.Setup(ds => ds.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName)).Returns<List<ViewEngine.FindViews.Message>>(null);
            storeMock.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns(new List<View.Render.Message>());

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.TabStore).Returns(storeMock.Object);

            var tab = new Metadata();

            Assert.DoesNotThrow(() => tab.GetData(contextMock.Object));
        }

        [Fact]
        public void HandleNullViewRenderMessageCollection()
        {
            var storeMock = new Mock<IDataStore>();
            storeMock.Setup(ds => ds.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName)).Returns(new List<ViewEngine.FindViews.Message>());
            storeMock.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns<List<View.Render.Message>>(null);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.TabStore).Returns(storeMock.Object);

            var tab = new Metadata();

            Assert.DoesNotThrow(() => tab.GetData(contextMock.Object));
        }

        [Fact]
        public void ReturnResult()
        {
            var storeMock = new Mock<IDataStore>();
             
            var actionValueProviderResult = new ValueProviderResult("action", null, null);
            var controllerValueProvider = new ValueProviderResult("controller", null, null);

            var valueProvider = new Mock<IValueProvider>();
            valueProvider.Setup(x => x.GetValue("action")).Returns(actionValueProviderResult);
            valueProvider.Setup(x => x.GetValue("controller")).Returns(controllerValueProvider);

            var controllerBase = new Mock<ControllerBase>();
            controllerBase.Object.ValueProvider = valueProvider.Object;

            var viewData = new Mock<ViewDataDictionary>();

            var viewContext = new ViewContext { Controller = controllerBase.Object, ViewData = viewData.Object };
            var textWriter = new StringWriter();
            var arguments = new View.Render.Arguments(new object[] { viewContext, textWriter });

            var timerResult = new TimerResult();
            var baseType2 = typeof(ViewRenderMessageShould);

            var mixinMock = new Mock<IViewCorrelationMixin>(); 
            var mixin = mixinMock.Object;

            var renderMessage = new View.Render.Message(arguments, timerResult, baseType2, mixin);

            var renderMessages = new List<View.Render.Message> { renderMessage };
            storeMock.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns(renderMessages);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.TabStore).Returns(storeMock.Object);

            var tab = new Metadata();

            var result = tab.GetData(contextMock.Object);

            var data = result as List<MetadataItemModel>;

            Assert.NotNull(data);
            Assert.NotEmpty(data);
        }

        [Fact]
        public void PersistOnMessagePublish()
        {
            var listMock = new Mock<IList<int>>();

            var storeMock = new Mock<IDataStore>();
            storeMock.Setup(s => s.Contains(It.IsAny<string>())).Returns(true);
            storeMock.Setup(s => s.Get<IList<int>>(It.IsAny<string>())).Returns(listMock.Object);

            var contextMock = new Mock<ITabSetupContext>();
            contextMock.Setup(c => c.GetTabStore()).Returns(storeMock.Object);

            Metadata.Persist(int.MaxValue, contextMock.Object);

            listMock.Verify(l => l.Add(It.IsAny<int>()));
        }

        [Fact]
        public void CreateKeyOnMessagePublish()
        {
            var listMock = new Mock<IList<int>>();

            var storeMock = new Mock<IDataStore>();
            storeMock.Setup(s => s.Contains(It.IsAny<string>())).Returns(false);
            storeMock.Setup(s => s.Get<IList<int>>(It.IsAny<string>())).Returns(listMock.Object);

            var contextMock = new Mock<ITabSetupContext>();
            contextMock.Setup(c => c.GetTabStore()).Returns(storeMock.Object);

            Metadata.Persist(int.MaxValue, contextMock.Object);

            listMock.Verify(l => l.Add(It.IsAny<int>()));
            storeMock.Verify(s => s.Set(typeof(int).FullName, It.IsAny<List<int>>()));
        }
    }
}