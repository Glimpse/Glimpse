using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Tab;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc.Tab
{
    public class ViewsShould
    {
        [Fact]
        public void Construct()
        {
            var views = new Views();

            Assert.NotNull(views as ITab);
            Assert.NotNull(views as ITabSetup);
        }

        [Fact]
        public void ExecuteOnEndRequest()
        {
            var views = new Views();

            Assert.Equal(RuntimeEvent.EndRequest, views.ExecuteOn);
        }

        [Fact]
        public void HaveHttpContextBase()
        {
            var views = new Views();
            Assert.Equal(typeof(HttpContextBase), views.RequestContextType);
        }

        [Fact]
        public void HaveProperName()
        {
            var views = new Views();
            Assert.Equal("Views", views.Name);
            
        }

        [Fact]
        public void SubscribeToViewMessageTypes()
        {
            var messageBrokerMock = new Mock<IMessageBroker>();
            var contextMock = new Mock<ITabSetupContext>();
            contextMock.Setup(c => c.MessageBroker).Returns(messageBrokerMock.Object);

            var tab = new Views();
            tab.Setup(contextMock.Object);

            messageBrokerMock.Verify(mb => mb.Subscribe(It.IsAny<Action<ViewEngine.FindViews.Message>>()));
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

            var tab = new Views();

            Assert.DoesNotThrow(()=>tab.GetData(contextMock.Object));
        }

        [Fact]
        public void HandleNullViewRenderMessageCollection()
        {
            var storeMock = new Mock<IDataStore>();
            storeMock.Setup(ds => ds.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName)).Returns(new List<ViewEngine.FindViews.Message>());
            storeMock.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns<List<View.Render.Message>>(null);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.TabStore).Returns(storeMock.Object);

            var tab = new Views();

            Assert.DoesNotThrow(() => tab.GetData(contextMock.Object));
        }
    }
}