using System;
using System.Diagnostics;
using System.Linq;
using Castle.DynamicProxy;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class CastleInvocationToAlternateMethodContextAdapterShould
    {
        [Fact]
        public void Construct()
        {
            var invocationMock = new Mock<IInvocation>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(invocationMock.Object, adapter.Invocation);
            Assert.Equal(loggerMock.Object, adapter.Logger);
        }

        [Fact]
        public void ThrowExceptionWithNullConstructorParameters()
        {
            var invocationMock = new Mock<IInvocation>();
            var loggerMock = new Mock<ILogger>();

            Assert.Throws<ArgumentNullException>(() => new CastleInvocationToAlternateMethodContextAdapter(null, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On));
            Assert.Throws<ArgumentNullException>(() => new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, null, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On));
        }

        [Fact]
        public void ReturnProxyFromInvocation()
        {
            var expected = new {Any = "Object"};
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.Proxy).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.Proxy);
            invocationMock.Verify(i=>i.Proxy, Times.Once());
        }

        [Fact]
        public void ReturnReturnValueFromInvocation()
        {
            var expected = new { Any = "Object" };
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.ReturnValue).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.ReturnValue);
            invocationMock.Verify(i => i.ReturnValue, Times.Once());
        }

        [Fact]
        public void SetReturnValueOnInvocation()
        {
            var expected = new { Any = "Object" };
            var invocationMock = new Mock<IInvocation>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            adapter.ReturnValue = expected;
            
            invocationMock.VerifySet(i=>i.ReturnValue = expected, Times.Once());
        }

        [Fact]
        public void ReturnInvocationTargetFromInvocation()
        {
            var expected = new { Any = "Object" };
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.InvocationTarget).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.InvocationTarget);
            invocationMock.Verify(i => i.InvocationTarget, Times.Once());
        }

        [Fact]
        public void ReturnMethodFromInvocation()
        {
            var expected = GetType().GetMethods().First();
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.Method).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.Method);
            invocationMock.Verify(i => i.Method, Times.Once());
        }

        [Fact]
        public void ReturnMethodInvocationTargetFromInvocation()
        {
            var expected = GetType().GetMethods().First();
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.MethodInvocationTarget).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.MethodInvocationTarget);
            invocationMock.Verify(i => i.MethodInvocationTarget, Times.Once());
        }

        [Fact]
        public void ReturnTargetTypeFromInvocation()
        {
            var expected = GetType();
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.TargetType).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.TargetType);
            invocationMock.Verify(i => i.TargetType, Times.Once());
        }

        [Fact]
        public void ReturnArgumentsFromInvocation()
        {
            var expected = new object[] {"One", 2};
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.Arguments).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.Arguments);
            invocationMock.Verify(i => i.Arguments, Times.Once());
        }

        [Fact]
        public void ReturnGenericArgumentsFromInvocation()
        {
            var expected = new Type[] { typeof(object), typeof(string)};
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.GenericArguments).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.GenericArguments);
            invocationMock.Verify(i => i.GenericArguments, Times.Once());
        }

        [Fact]
        public void SetArgumentValueOnInvocation()
        {
            var invocationMock = new Mock<IInvocation>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);
            adapter.SetArgumentValue(0, "thing");

            invocationMock.Verify(i => i.SetArgumentValue(0,"thing"), Times.Once());
        }

        [Fact]
        public void ReturnGetArgumentValueFromInvocation()
        {
            var expected = new {Any="Object"};
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.GetArgumentValue(0)).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.GetArgumentValue(0));
            invocationMock.Verify(i => i.GetArgumentValue(0), Times.Once());
        }

        [Fact]
        public void ReturnGetConcreteMethodFromInvocation()
        {
            var expected = GetType().GetMethods().First();
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.GetConcreteMethod()).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.GetConcreteMethod());
            invocationMock.Verify(i => i.GetConcreteMethod(), Times.Once());
        }

        [Fact]
        public void ReturnGetConcreteMethodInvocatedTargetFromInvocation()
        {
            var expected = GetType().GetMethods().First();
            var invocationMock = new Mock<IInvocation>();
            invocationMock.Setup(i => i.GetConcreteMethodInvocationTarget()).Returns(expected);
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            Assert.Equal(expected, adapter.GetConcreteMethodInvocationTarget());
            invocationMock.Verify(i => i.GetConcreteMethodInvocationTarget(), Times.Once());
        }

        [Fact]
        public void ProceedOnInvocation()
        {
            var invocationMock = new Mock<IInvocation>();
            var loggerMock = new Mock<ILogger>();

            var adapter = new CastleInvocationToAlternateMethodContextAdapter(invocationMock.Object, loggerMock.Object, new Mock<IMessageBroker>().Object, new Mock<IProxyFactory>().Object, () => new ExecutionTimer(Stopwatch.StartNew()), () => RuntimePolicy.On);

            adapter.Proceed();
            invocationMock.Verify(i => i.Proceed(), Times.Once());
        }
    }
}