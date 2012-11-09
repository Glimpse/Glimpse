using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Extensibility;
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
    public class MetadataShould
    {
        [Theory, AutoMock]
        public void Construct(Metadata sut)
        {
            Assert.NotNull(sut as ITab);
            Assert.NotNull(sut as ITabSetup);
        }

        [Theory, AutoMock]
        public void ExecuteOnEndRequest(Metadata sut)
        {
            Assert.Equal(RuntimeEvent.EndRequest, sut.ExecuteOn);
        }

        [Theory, AutoMock]
        public void HaveHttpContextBase(Metadata sut)
        {
            Assert.Equal(typeof(HttpContextBase), sut.RequestContextType);
        }

        [Theory, AutoMock]
        public void HaveProperName(Metadata sut)
        {
            Assert.Equal("Metadata", sut.Name);
        }

        [Theory, AutoMock]
        public void SubscribeToViewMessageTypes(Metadata sut, ITabSetupContext context)
        {
            sut.Setup(context);

            context.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<View.Render.Message>>()));
        }

        [Theory, AutoMock]
        public void HandleNullFindViewMessageCollection(Metadata sut, ITabContext context)
        {
            context.TabStore.Setup(ds => ds.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName)).Returns<List<ViewEngine.FindViews.Message>>(null);
            context.TabStore.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns(new List<View.Render.Message>());

            Assert.DoesNotThrow(() => sut.GetData(context));
        }

        [Theory, AutoMock]
        public void HandleNullViewRenderMessageCollection(Metadata sut, ITabContext context)
        {
            context.TabStore.Setup(ds => ds.Get<List<ViewEngine.FindViews.Message>>(typeof(ViewEngine.FindViews.Message).FullName)).Returns(new List<ViewEngine.FindViews.Message>());
            context.TabStore.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns<List<View.Render.Message>>(null);

            Assert.DoesNotThrow(() => sut.GetData(context));
        }

        [Theory, AutoMock]
        public void ReturnResult(Metadata sut, ITabContext context, View.Render.Arguments renderArgs, TimerResult timerResult, IViewCorrelationMixin mixin)
        {
            var renderMessage = new View.Render.Message(
                input: renderArgs, 
                timing: timerResult, 
                baseType: typeof(ViewRenderMessageShould), 
                viewCorrelation: mixin);

            context.TabStore.Setup(ds => ds.Get<List<View.Render.Message>>(typeof(View.Render.Message).FullName)).Returns(new List<View.Render.Message> { renderMessage });

            var result = sut.GetData(context) as List<MetadataItemModel>;

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Theory, AutoMock]
        public void PersistOnMessagePublish(ITabSetupContext context, IList<int> list)
        {
            context.GetTabStore().Setup(s => s.Contains(It.IsAny<string>())).Returns(true);
            context.GetTabStore().Setup(s => s.Get<IList<int>>(It.IsAny<string>())).Returns(list);

            Metadata.Persist(int.MaxValue, context);

            list.Verify(l => l.Add(It.IsAny<int>()));
        }

        [Theory, AutoMock]
        public void CreateKeyOnMessagePublish(ITabSetupContext context, IList<int> list)
        {
            context.GetTabStore().Setup(s => s.Contains(It.IsAny<string>())).Returns(false);
            context.GetTabStore().Setup(s => s.Get<IList<int>>(It.IsAny<string>())).Returns(list);


            Metadata.Persist(int.MaxValue, context);

            list.Verify(l => l.Add(It.IsAny<int>()));
            context.GetTabStore().Verify(s => s.Set(typeof(int).FullName, It.IsAny<List<int>>()));
        }
    }
}