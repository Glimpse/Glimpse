using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Moq;
using Xunit;

namespace Glimpse.Test.Core.Extensibility
{
    public class AlternateImplementationSelectorShould
    {
        [Fact]
        public void ConstructWithMethodImplementations()
        {
            var loggerMock = new Mock<ILogger>();
            var alternateMock1 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof(IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof(AlternateImplementationSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var implementations = new List<AlternateImplementationToCastleInterceptorAdapter<IDisposable>>
                                                                         {
                                                                             new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock1.Object, loggerMock.Object), new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock2.Object, loggerMock.Object),
                                                                         };


            var selector = new AlternateImplementationSelector<IDisposable>(implementations);

            Assert.Equal(implementations, selector.MethodImplementations);
        }

        [Fact]
        public void ReturnMatchingInterceptors()
        {
            var loggerMock = new Mock<ILogger>();
            var alternateMock1 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof (IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof (AlternateImplementationSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var selector = new AlternateImplementationSelector<IDisposable>(new List<AlternateImplementationToCastleInterceptorAdapter<IDisposable>> 
                                                                                   {
                                                                                       new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock1.Object, loggerMock.Object),
                                                                                       new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock2.Object, loggerMock.Object),
                                                                                   });

            var result = selector.SelectInterceptors(null, typeof (IDisposable).GetMethod("Dispose"), null);

            Assert.Equal(1, result.Length);
        }

        [Fact]
        public void ReturnEmptyArrayWithoutMatch()
        {
            var loggerMock = new Mock<ILogger>();
            var alternateMock1 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock1.Setup(a => a.MethodToImplement).Returns(typeof(IDisposable).GetMethod("Dispose"));

            var alternateMock2 = new Mock<IAlternateImplementation<IDisposable>>();
            alternateMock2.Setup(a => a.MethodToImplement).Returns(typeof(AlternateImplementationSelectorShould).GetMethod("ReturnMatchingInterceptors"));

            var selector = new AlternateImplementationSelector<IDisposable>(new List<AlternateImplementationToCastleInterceptorAdapter<IDisposable>> 
                                                                                   {
                                                                                       new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock1.Object, loggerMock.Object),
                                                                                       new AlternateImplementationToCastleInterceptorAdapter<IDisposable>(alternateMock2.Object, loggerMock.Object),
                                                                                   });

            var result = selector.SelectInterceptors(null, typeof(AlternateImplementationSelectorShould).GetMethod("ReturnEmptyArrayWithoutMatch"), null);

            Assert.Empty(result);
        }

        [Fact]
        public void EquateInstancesWithSimilarImplementations()
        {
            var loggerMock = new Mock<ILogger>();

            var alternateMock1 = new Mock<IAlternateImplementation<ITab>>();
            var alternateMock2 = new Mock<IAlternateImplementation<ITab>>();

            var implementations1 = new List<AlternateImplementationToCastleInterceptorAdapter<ITab>>
                                       {
                                           new AlternateImplementationToCastleInterceptorAdapter<ITab>(alternateMock1.Object, loggerMock.Object),
                                           new AlternateImplementationToCastleInterceptorAdapter<ITab>(alternateMock2.Object, loggerMock.Object),
                                       };
            var implementations2 = new List<AlternateImplementationToCastleInterceptorAdapter<ITab>>
                                       {
                                           new AlternateImplementationToCastleInterceptorAdapter<ITab>(alternateMock2.Object, loggerMock.Object),
                                           new AlternateImplementationToCastleInterceptorAdapter<ITab>(alternateMock1.Object, loggerMock.Object), //Order swapped
                                       };

            var selector1 = new AlternateImplementationSelector<ITab>(implementations1);
            var selector2 = new AlternateImplementationSelector<ITab>(implementations2);

            Assert.True(selector1.Equals(selector2));
            Assert.Equal(selector1.GetHashCode(), selector2.GetHashCode());
        }

        [Fact]
        public void NotEquateInstancesWithDifferentImplementations()
        {
            var loggerMock = new Mock<ILogger>();

            var alternateMock1 = new Mock<IAlternateImplementation<ITab>>();
            var alternateMock2 = new Mock<IAlternateImplementation<ITab>>();

            var implementations1 = new List<AlternateImplementationToCastleInterceptorAdapter<ITab>>
                                       {
                                           new AlternateImplementationToCastleInterceptorAdapter<ITab>(alternateMock1.Object, loggerMock.Object),
                                           new AlternateImplementationToCastleInterceptorAdapter<ITab>(alternateMock2.Object, loggerMock.Object),
                                       };
            var implementations2 = new List<AlternateImplementationToCastleInterceptorAdapter<ITab>>
                                       {
                                           new AlternateImplementationToCastleInterceptorAdapter<ITab>(alternateMock1.Object, loggerMock.Object), //Less items
                                       };

            var selector1 = new AlternateImplementationSelector<ITab>(implementations1);
            var selector2 = new AlternateImplementationSelector<ITab>(implementations2);

            Assert.False(selector1.Equals(selector2));
            Assert.NotEqual(selector1.GetHashCode(), selector2.GetHashCode());
            Assert.NotEqual(selector1.GetHashCode(), selector2.GetHashCode());
        }
    }
}