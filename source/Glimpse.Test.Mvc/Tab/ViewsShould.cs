using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glimpse.Core2.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.Tab;
using Glimpse.Test.Mvc.AlternateImplementation;
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

        [Fact]
        public void ReturnResult()
        {
            var storeMock = new Mock<IDataStore>();

            var input = new ViewEngine.FindViews.Arguments(new object[] { new ControllerContext(), "ViewName", false }, true);
            var output = new ViewEngineResult(Enumerable.Empty<string>());
            var timing = new TimerResult();
            var baseType1 = typeof(string);
            var isPartial = false;
            var id = Guid.NewGuid();

            var findViewMessage = new ViewEngine.FindViews.Message(input, output, timing, baseType1, isPartial, id);

            var findViewMessages = new List<ViewEngine.FindViews.Message>
                               {
                                   findViewMessage
                               };
            storeMock.Setup(ds => ds.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName)).Returns(findViewMessages);

            var viewContext = new ViewContext {ViewData = new ViewDataDictionary(), TempData = new TempDataDictionary()};
            var textWriter = new StringWriter();
            var arguments = new View.Render.Arguments(new object[] { viewContext, textWriter });

            var timerResult = new TimerResult();
            var baseType2 = typeof(ViewRenderMessageShould);

            var mixinMock = new Mock<View.Render.IMixin>();
            mixinMock.Setup(m => m.ViewEngineFindCallId).Returns(id);
            var mixin = mixinMock.Object;

            var renderMessage = new View.Render.Message(arguments, timerResult, baseType2, mixin);

            var renderMessages = new List<View.Render.Message>{renderMessage};
            storeMock.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns(renderMessages);

            var contextMock = new Mock<ITabContext>();
            contextMock.Setup(c => c.TabStore).Returns(storeMock.Object);

            var tab = new Views();

            var result = tab.GetData(contextMock.Object);

            var data = result as List<ViewsModel>;

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

            Views.Persist(int.MaxValue, contextMock.Object);

            listMock.Verify(l=>l.Add(It.IsAny<int>()));
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

            Views.Persist(int.MaxValue, contextMock.Object);

            listMock.Verify(l => l.Add(It.IsAny<int>()));
            storeMock.Verify(s => s.Set(typeof (int).FullName, It.IsAny<List<int>>()));
        }
    }
}