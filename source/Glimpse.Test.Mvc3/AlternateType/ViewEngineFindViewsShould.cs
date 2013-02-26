using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateType;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateType
{
    public class ViewEngineFindViewsShould
    {
        private readonly IFixture fixture = new Fixture();

        [Theory, AutoMock]
        public void ReturnAllMethodImplementationsWithAllMethods(ViewEngine sut)
        {
            var allMethods = sut.AllMethods;

            Assert.Equal(2, allMethods.Count());
        }

        [Theory, AutoMock]
        public void Construct(AlternateType<IView> alternateView)
        {
            var sut = new ViewEngine.FindViews(false, alternateView);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IAlternateMethod>(sut);
        }

        [Theory, AutoMock]
        public void MethodToImplementIsRight(AlternateType<IView> alternateView)
        {
            var sut1 = new ViewEngine.FindViews(false, alternateView);
            Assert.Equal("FindView", sut1.MethodToImplement.Name);

            var sut2 = new ViewEngine.FindViews(true, alternateView);
            Assert.Equal("FindPartialView", sut2.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedIfRuntimePolicyIsOff(ViewEngine.FindViews sut, IAlternateMethodContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
        }

        [Theory, AutoMock]
        public void PublishMessagesIfRuntimePolicyIsOnAndViewNotFound(ViewEngine.FindViews sut, IAlternateMethodContext context, ControllerContext controllerContext)
        {
            context.Setup(c => c.Arguments).Returns(GetArguments(controllerContext));
            context.Setup(c => c.TargetType).Returns(typeof(int));
            context.Setup(c => c.ReturnValue).Returns(new ViewEngineResult(Enumerable.Empty<string>()));

            sut.NewImplementation(context);

            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ViewEngine.FindViews.Message>())); 
        }

        [Theory, AutoMock]
        public void PublishMessagesIfRuntimePolicyIsOnAndViewIsFound([Frozen] IProxyFactory proxyFactory, ViewEngine.FindViews sut, IAlternateMethodContext context, IView view, IViewEngine engine, ControllerContext controllerContext)
        {
            context.Setup(c => c.Arguments).Returns(GetArguments(controllerContext));
            context.Setup(c => c.ReturnValue).Returns(new ViewEngineResult(view, engine));
            context.Setup(c => c.TargetType).Returns(typeof(int));
            proxyFactory.Setup(p => p.IsWrapInterfaceEligible<IView>(It.IsAny<Type>())).Returns(true);
            proxyFactory.Setup(p => 
                    p.WrapInterface(
                        It.IsAny<IView>(), 
                        It.IsAny<IEnumerable<IAlternateMethod>>(),
                        It.IsAny<IEnumerable<object>>()))
                    .Returns(view);

            sut.NewImplementation(context);

            proxyFactory.Verify(p => p.IsWrapInterfaceEligible<IView>(It.IsAny<Type>()));
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
            context.VerifySet(c => c.ReturnValue = It.IsAny<ViewEngineResult>());
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ViewEngine.FindViews.Message>())); 
        }

        private object[] GetArguments(ControllerContext controllerContext)
        {
            return new object[]
                {
                    controllerContext, 
                    fixture.CreateAnonymous("ViewName"), 
                    fixture.CreateAnonymous("MasterName"), 
                    fixture.CreateAnonymous<bool>()
                };
        }
    }
}