using System;
using System.Collections.Generic;
using System.Reflection;
using Glimpse.Core.Extensibility;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Message
{
    public class ActionMessageShould
    {
        [Theory, AutoMock]
        public void Construct(TimerResult timerResult, string controllerName, string actionName, bool isChildAction, Type executedType, MethodInfo method)
        {
            var message = new ActionMessage(timerResult, controllerName, actionName, isChildAction, executedType, method);

            Assert.Equal(timerResult.Duration, message.Duration);
            Assert.Equal(timerResult.Offset, message.Offset);
            Assert.Equal(timerResult.StartTime, message.StartTime);
            Assert.Equal(executedType, message.ExecutedType);
            Assert.Equal(method, message.ExecutedMethod);
            Assert.Equal(controllerName, message.ControllerName);
            Assert.Equal(actionName, message.ActionName);
            Assert.Equal(string.Format("{0}:{1}", controllerName, actionName), message.EventName);
        }

        [Theory, AutoMock]
        public void BuildDetails(TimerResult timerResult, string controllerName, string actionName, bool isChildAction, Type executedType, MethodInfo method, string eventName, string eventCategory)
        {
            var message = new ActionMessage(timerResult, controllerName, actionName, isChildAction, executedType, method, eventName, eventCategory);

            var dictionary = new Dictionary<string, object>();
            message.BuildDetails(dictionary);

            Assert.Equal(3, dictionary.Count); 
        }
    }
}