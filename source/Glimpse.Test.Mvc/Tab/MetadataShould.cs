using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Model;
using Glimpse.Mvc.Tab;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.Tab
{
    public class MetadataShould
    {
        [Theory, AutoMock]
        public void Construct(Metadata sut)
        {
            Assert.IsAssignableFrom<ITab>(sut);
            Assert.IsAssignableFrom<ITabSetup>(sut);
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
            context.TabStore.Setup(ds => ds.Get(typeof(ViewEngine.FindViews.Message).FullName)).Returns<List<ViewEngine.FindViews.Message>>(null);
            context.TabStore.Setup(ds => ds.Get(typeof(View.Render.Message).FullName)).Returns(new List<View.Render.Message>());

            Assert.DoesNotThrow(() => sut.GetData(context));
        }

        [Theory, AutoMock]
        public void HandleNullViewRenderMessageCollection(Metadata sut, ITabContext context)
        {
            context.TabStore.Setup(ds => ds.Get(typeof(ViewEngine.FindViews.Message).FullName)).Returns(new List<ViewEngine.FindViews.Message>());
            context.TabStore.Setup(ds => ds.Get(typeof(View.Render.Message).FullName)).Returns<List<View.Render.Message>>(null);

            Assert.DoesNotThrow(() => sut.GetData(context));
        }

        [Theory, AutoMock]
        public void ReturnResult(Metadata sut, ITabContext context, View.Render.Message renderMessage)
        { 
            context.TabStore.Setup(ds => ds.Contains(typeof(IList<View.Render.Message>).AssemblyQualifiedName)).Returns(true);
            context.TabStore.Setup(ds => ds.Get(typeof(IList<View.Render.Message>).AssemblyQualifiedName)).Returns(new List<View.Render.Message> { renderMessage });

            var result = sut.GetData(context) as List<MetadataItemModel>;

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}