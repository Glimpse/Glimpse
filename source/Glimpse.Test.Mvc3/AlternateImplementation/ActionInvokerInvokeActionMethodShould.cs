using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc3.AlternateImplementation;
using Glimpse.Test.Mvc3.TestDoubles;
using Moq;
using Xunit;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionInvokerInvokeActionMethodShould
    {
        [Fact]
        public void ImplementProperMethod()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();

            var implementation = new ActionInvoker.InvokeActionMethod(policyStrategy, timerStrategy, brokerMock.Object);

            Assert.Equal("InvokeActionMethod", implementation.MethodToImplement.Name);
        }

        [Fact]
        public void ProccedAndReturnWithRuntimePolicyOff()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.Off;
            Func<IExecutionTimer> timerStrategy = () => new Mock<IExecutionTimer>().Object;
            var brokerMock = new Mock<IMessageBroker>();

            var contextMock = new Mock<IAlternateImplementationContext>();

            var implementation = new ActionInvoker.InvokeActionMethod(policyStrategy, timerStrategy, brokerMock.Object);
            implementation.NewImplementation(contextMock.Object);


            contextMock.Verify(c=>c.Proceed());
            brokerMock.Verify(b=>b.Publish(It.IsAny<ActionInvoker.InvokeActionMethod.Message>()), Times.Never());
        }

        [Fact]
        public void PublishMessageWithRuntimePolicyOn()
        {
            Func<RuntimePolicy> policyStrategy = () => RuntimePolicy.On;
            var timerMock = new Mock<IExecutionTimer>();
            Func<IExecutionTimer> timerStrategy = () => timerMock.Object;
            var brokerMock = new Mock<IMessageBroker>();

            var actionDescriptorMock = new Mock<ActionDescriptor>();
            actionDescriptorMock.Setup(a => a.ControllerDescriptor).Returns(new ReflectedControllerDescriptor(typeof(DummyController)));

            var contextMock = new Mock<IAlternateImplementationContext>();
            contextMock.Setup(c => c.ReturnValue).Returns(new ContentResult());
            contextMock.Setup(c => c.Arguments).Returns(new object[]
                                                            {
                                                                new ControllerContext(),
                                                                actionDescriptorMock.Object,
                                                                new Dictionary<string,object>(),
                                                            });

            var implementation = new ActionInvoker.InvokeActionMethod(policyStrategy, timerStrategy, brokerMock.Object);
            implementation.NewImplementation(contextMock.Object);

            timerMock.Verify(t=>t.Time(It.IsAny<Action>()));
            brokerMock.Verify(b => b.Publish(It.IsAny<ActionInvoker.InvokeActionMethod.Message>()));
        } 
    }
}