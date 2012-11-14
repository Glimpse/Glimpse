using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Glimpse.Core;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Moq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ViewEngineFindViewsShould
    {
        private readonly IFixture fixture = new Fixture();

        [Theory, AutoMock]
        public void ReturnAllMethodImplementationsWithAllMethods(ViewEngine sut)
        {
            var allMethods = sut.AllMethods();

            Assert.Equal(2, allMethods.Count());
        }

        [Theory, AutoMock]
        public void Construct(Alternate<IView> alternateView)
        {
            var sut = new ViewEngine.FindViews(false, alternateView);

            Assert.NotNull(sut);
            Assert.IsAssignableFrom<IAlternateImplementation<IViewEngine>>(sut);
        }

        [Theory, AutoMock]
        public void MethodToImplementIsRight(Alternate<IView> alternateView)
        {
            var sut1 = new ViewEngine.FindViews(false, alternateView);
            Assert.Equal("FindView", sut1.MethodToImplement.Name);

            var sut2 = new ViewEngine.FindViews(true, alternateView);
            Assert.Equal("FindPartialView", sut2.MethodToImplement.Name);
        }

        [Theory, AutoMock]
        public void ProceedIfRuntimePolicyIsOff(ViewEngine.FindViews sut, IAlternateImplementationContext context)
        {
            context.Setup(c => c.RuntimePolicyStrategy).Returns(() => RuntimePolicy.Off);

            sut.NewImplementation(context);

            context.Verify(c => c.Proceed());
        }

        [Theory, AutoMock]
        public void PublishMessagesIfRuntimePolicyIsOnAndViewNotFound(ViewEngine.FindViews sut, IAlternateImplementationContext context, ControllerContext controllerContext)
        {
            context.Setup(c => c.Arguments).Returns(GetArguments(controllerContext));
            context.Setup(c => c.TargetType).Returns(typeof(int));
            context.Setup(c => c.ReturnValue).Returns(new ViewEngineResult(Enumerable.Empty<string>()));

            sut.NewImplementation(context);

            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ViewEngine.FindViews.Message>()));
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ViewEngine.FindViews.EventMessage>()));
        }

        [Theory, AutoMock]
        public void PublishMessagesIfRuntimePolicyIsOnAndViewIsFound(ViewEngine.FindViews sut, IAlternateImplementationContext context, IView view, IViewEngine engine, ControllerContext controllerContext)
        {
            context.Setup(c => c.Arguments).Returns(GetArguments(controllerContext));
            context.Setup(c => c.ReturnValue).Returns(new ViewEngineResult(view, engine));
            context.Setup(c => c.TargetType).Returns(typeof(int));
            context.ProxyFactory.Setup(p => p.IsProxyable(It.IsAny<object>())).Returns(true);
            context.ProxyFactory.Setup(p => 
                    p.CreateProxy(
                        It.IsAny<IView>(), 
                        It.IsAny<IEnumerable<IAlternateImplementation<IView>>>(), 
                        It.IsAny<object>()))
                    .Returns(view);

            sut.NewImplementation(context);

            context.ProxyFactory.Verify(p => p.IsProxyable(It.IsAny<object>()));
            context.Logger.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()));
            context.VerifySet(c => c.ReturnValue = It.IsAny<ViewEngineResult>());
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ViewEngine.FindViews.Message>()));
            context.MessageBroker.Verify(b => b.Publish(It.IsAny<ViewEngine.FindViews.EventMessage>()));
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