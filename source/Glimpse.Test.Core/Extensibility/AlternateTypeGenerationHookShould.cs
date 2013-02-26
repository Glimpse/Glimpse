using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class AlternateTypeGenerationHookShould
    {
        [Fact]
        public void ConstructWithParameters()
        {
            var alternateImplementations = Enumerable.Empty<IAlternateMethod>();
            var loggerMock = new Mock<ILogger>();

            var hook = new AlternateTypeGenerationHook<ITab>(alternateImplementations, loggerMock.Object);

            Assert.Equal(alternateImplementations, hook.MethodImplementations);
            Assert.Equal(loggerMock.Object, hook.Logger);
        }

        [Fact]
        public void ThrowWithNullMethodImplementationsParameter()
        {
            var loggerMock = new Mock<ILogger>();

            Assert.Throws<ArgumentNullException>(() => new AlternateTypeGenerationHook<ITab>(null, loggerMock.Object));
        }

        [Fact]
        public void ThrowWithNullLoggerParameter()
        {
            var alternateImplementations = Enumerable.Empty<IAlternateMethod>();

            Assert.Throws<ArgumentNullException>(() => new AlternateTypeGenerationHook<ITab>(alternateImplementations, null));
        }

        [Fact]
        public void LogWhenMethodsInspected()
        {
            var alternateImplementations = Enumerable.Empty<IAlternateMethod>();
            var loggerMock = new Mock<ILogger>();

            var hook = new AlternateTypeGenerationHook<ITab>(alternateImplementations, loggerMock.Object);

            hook.MethodsInspected();

            loggerMock.Verify(l=>l.Debug(It.IsAny<string>(), It.IsAny<Type>()), Times.Once());
        }

        [Fact]
        public void LogWhenNonProxableMemberNotification()
        {
            var alternateImplementations = Enumerable.Empty<IAlternateMethod>();
            var loggerMock = new Mock<ILogger>();

            var hook = new AlternateTypeGenerationHook<ITab>(alternateImplementations, loggerMock.Object);

            var type = typeof (AlternateTypeGenerationHookShould);
            var memberInfo = type.GetMember("LogWhenNonProxableMemberNotification")[0];

            hook.NonProxyableMemberNotification(type, memberInfo);

            loggerMock.Verify(l => l.Debug(It.IsAny<string>(), memberInfo.Name, type), Times.Once());
        }

        [Fact]
        public void CanInterceptMethods()
        {
            var type = typeof (AlternateTypeGenerationHookShould);
            var methodInfo = type.GetMethod("CanInterceptMethods");
            var implementationMock = new Mock<IAlternateMethod>();
            implementationMock.Setup(i => i.MethodToImplement).Returns(methodInfo);

            var alternateImplementations = new List<IAlternateMethod>();
            alternateImplementations.Add(implementationMock.Object);
            
            var loggerMock = new Mock<ILogger>();

            var hook = new AlternateTypeGenerationHook<ITab>(alternateImplementations, loggerMock.Object);

            var result = hook.ShouldInterceptMethod(type, methodInfo);

            Assert.True(result);
        }

        [Fact]
        public void NotInterceptMethodsWithMismatch()
        {
            var type = typeof(AlternateTypeGenerationHookShould);
            var methodInfo = type.GetMethod("CanInterceptMethods");
            var implementationMock = new Mock<IAlternateMethod>();
            implementationMock.Setup(i => i.MethodToImplement).Returns(methodInfo);

            var alternateImplementations = new List<IAlternateMethod>();
            alternateImplementations.Add(implementationMock.Object);

            var loggerMock = new Mock<ILogger>();

            var hook = new AlternateTypeGenerationHook<ITab>(alternateImplementations, loggerMock.Object);

            var result = hook.ShouldInterceptMethod(type, type.GetMethod("NotInterceptMethodsWithMismatch"));

            Assert.False(result);
        }

        [Fact]
        public void ReturnNonMatchingHashCodesWithDifferentCollections()
        {
            var implementationMock1 = new Mock<IAlternateMethod>();
            var implementationMock2 = new Mock<IAlternateMethod>();

            var implementations1 = new List<IAlternateMethod>
                                      {
                                          implementationMock1.Object,
                                          implementationMock2.Object
                                      };

            var implementations2 = new List<IAlternateMethod> 
                                      {
                                          implementationMock1.Object //One less object
                                      };
            var loggerMock = new Mock<ILogger>();

            var hook1 = new AlternateTypeGenerationHook<ITab>(implementations1, loggerMock.Object);
            var hook2 = new AlternateTypeGenerationHook<ITab>(implementations2, loggerMock.Object);

            Assert.NotEqual(hook2.GetHashCode(), hook1.GetHashCode());
            Assert.False(hook2.Equals(hook1));
            
        }
    }
}