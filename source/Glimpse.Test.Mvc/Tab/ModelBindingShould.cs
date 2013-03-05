using System;
using System.Web.Mvc;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Tab;
using Glimpse.Test.Common;
using Moq;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc.Tab
{
    public class ModelBindingShould
    {
        [Fact]
        public void Construct()
        {
            var sut = new ModelBinding();

            Assert.IsAssignableFrom<AspNetTab>(sut);
            Assert.IsAssignableFrom<ITabSetup>(sut);
        }

        [Theory, AutoMock]
        public void ReturnProperName(ModelBinding sut)
        {
            Assert.Equal("Model Binding", sut.Name);
        }

        [Theory, AutoMock]
        public void SubscribeToMessages(ModelBinding sut, ITabSetupContext context)
        {
            sut.Setup(context);

            context.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<ValueProvider<IValueProvider>.GetValue.Message>>()));
            context.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<ValueProvider<IUnvalidatedValueProvider>.GetValue.Message>>()));
            context.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<ModelBinder.BindModel.Message>>()));
            context.MessageBroker.Verify(mb => mb.Subscribe(It.IsAny<Action<ModelBinder.BindProperty.Message>>()));
        }
    }
}