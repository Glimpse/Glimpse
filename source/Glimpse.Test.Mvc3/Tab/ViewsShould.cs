using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.Tab;
using Glimpse.Test.Common;
using Glimpse.Test.Mvc3.AlternateImplementation;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Tab
{
    public class ViewsShould
    {
        [Theory, AutoMock]
        public void Construct(Views sut)
        {
            Assert.IsAssignableFrom<ITab>(sut);
            Assert.IsAssignableFrom<ITabSetup>(sut);
        }

        [Theory, AutoMock]
        public void ExecuteOnEndRequest(Views sut)
        {
            Assert.Equal(RuntimeEvent.EndRequest, sut.ExecuteOn);
        }

        [Theory, AutoMock]
        public void HaveHttpContextBase(Views sut)
        {
            Assert.Equal(typeof(HttpContextBase), sut.RequestContextType);
        }

        [Theory, AutoMock]
        public void HaveProperName(Views sut)
        {
            Assert.Equal("Views", sut.Name);
        }

        [Theory, AutoMock]
        public void SubscribeToViewMessageTypes(Views sut, ITabSetupContext context)
        {
            sut.Setup(context);

            context.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<ViewEngine.FindViews.Message>>()));
            context.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<View.Render.Message>>()));
        }

        [Theory, AutoMock]
        public void HandleNullFindViewMessageCollection(Views sut, ITabContext context)
        {
            context.TabStore.Setup(ds => ds.Get(typeof(ViewEngine.FindViews.Message).FullName)).Returns<List<ViewEngine.FindViews.Message>>(null);
            context.TabStore.Setup(ds => ds.Get(typeof(View.Render.Message).FullName)).Returns(new List<View.Render.Message>());

            Assert.DoesNotThrow(() => sut.GetData(context));
        }

        [Theory, AutoMock]
        public void HandleNullViewRenderMessageCollection(Views sut, ITabContext context)
        {
            context.TabStore.Setup(ds => ds.Get(typeof(ViewEngine.FindViews.Message).FullName)).Returns(new List<ViewEngine.FindViews.Message>());
            context.TabStore.Setup(ds => ds.Get(typeof(View.Render.Message).FullName)).Returns<List<View.Render.Message>>(null);

            Assert.DoesNotThrow(() => sut.GetData(context));
        }

        [Theory, AutoMock]
        public void ReturnResult(Views sut, ITabContext context, View.Render.Arguments renderArgs, ViewEngine.FindViews.Arguments findViewArgs, ViewEngineResult viewEngineResult, IViewCorrelationMixin mixin, TimerResult timerResult, Guid id)
        {
            var findViewMessage = new ViewEngine.FindViews.Message(
                input: findViewArgs, 
                output: viewEngineResult, 
                timing: timerResult, 
                baseType: typeof(string), 
                isPartial: false, 
                id: id);

            context.TabStore.Setup(ds => ds.Get(typeof(ViewEngine.FindViews.Message).FullName)).Returns(new List<ViewEngine.FindViews.Message> { findViewMessage });

            mixin.Setup(m => m.ViewEngineFindCallId).Returns(id);

            var renderMessage = new View.Render.Message(
                input: renderArgs, 
                timing: timerResult, 
                baseType: typeof(ViewRenderMessageShould), 
                viewCorrelation: mixin);

            context.TabStore.Setup(ds => ds.Get(typeof(View.Render.Message).FullName)).Returns(new List<View.Render.Message> { renderMessage });

            var result = sut.GetData(context) as List<ViewsModel>;

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory, AutoMock]
        public void PersistOnMessagePublish(ITabSetupContext context, IList<int> list)
        {
            context.GetTabStore().Setup(s => s.Contains(It.IsAny<string>())).Returns(true);
            context.GetTabStore().Setup(s => s.Get(It.IsAny<string>())).Returns(list);

            Views.Persist(int.MaxValue, context);

            list.Verify(l => l.Add(It.IsAny<int>()));
        }

        [Theory, AutoMock]
        public void CreateKeyOnMessagePublish(ITabSetupContext context, IList<int> list)
        {
            context.GetTabStore().Setup(s => s.Contains(It.IsAny<string>())).Returns(false);
            context.GetTabStore().Setup(s => s.Get(It.IsAny<string>())).Returns(list);

            Views.Persist(int.MaxValue, context);

            list.Verify(l => l.Add(It.IsAny<int>()));
            context.GetTabStore().Verify(s => s.Set(typeof(int).FullName, It.IsAny<List<int>>()));
        }
    }
}