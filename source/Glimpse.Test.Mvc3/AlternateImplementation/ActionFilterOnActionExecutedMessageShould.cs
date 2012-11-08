using System;
using System.Reflection;
using System.Web.Mvc;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.AlternateImplementation;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.AlternateImplementation
{
    public class ActionFilterOnActionExecutedMessageShould
    {
        [Theory, AutoMock]
        public void Construct(ActionExecutedContext context, Type filterType, MethodInfo methodInfo, TimerResult timerResult)
        {
            var sut = new ActionFilter.OnActionExecuted.Message(context, filterType, methodInfo, timerResult);

            Assert.Equal(context.ActionDescriptor.ActionName, sut.ActionName);
            Assert.Equal(context.ActionDescriptor.ControllerDescriptor.ControllerName, sut.ControllerName);
            Assert.Equal(context.Exception.GetType(), sut.ExceptionType);
            Assert.Equal(context.Canceled, sut.IsCanceled);
            Assert.Equal(context.ExceptionHandled, sut.ExceptionHandled);
            Assert.Equal(context.Result.GetType(), sut.ResultType);
            Assert.Equal(filterType, sut.ExecutedType);
            Assert.Equal(methodInfo, sut.ExecutedMethod);
            Assert.Equal(timerResult.Duration, sut.Duration);
            Assert.Equal(timerResult.Offset, sut.Offset);
        }

        [Theory, AutoMock]
        public void ConstructWithMissingException(ActionExecutedContext context, Type filterType, MethodInfo methodInfo, TimerResult timerResult)
        {
            context.Exception = null;

            var sut = new ActionFilter.OnActionExecuted.Message(context, filterType, methodInfo, timerResult);

            Assert.Null(sut.ExceptionType);
        }

        [Theory, AutoMock]
        public void ConstructWithMissingResult(ActionExecutedContext context, Type filterType, MethodInfo methodInfo, TimerResult timerResult)
        {
            context.Result = null;

            var sut = new ActionFilter.OnActionExecuted.Message(context, filterType, methodInfo, timerResult);

            Assert.NotNull(sut.ResultType);
            Assert.Equal(typeof(EmptyResult), sut.ResultType);
        }
    }
}