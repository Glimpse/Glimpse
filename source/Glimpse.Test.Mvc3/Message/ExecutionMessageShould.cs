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
    public class ExecutionMessageShould
    {
        [Theory, AutoMock]
        public void Construct(TimerResult timerResult, bool isChildAction, Type executedType, MethodInfo method, string eventName, string eventCategory)
        {
            var message = new ActionBaseMessage(timerResult, isChildAction, executedType, method, eventName, eventCategory);

            Assert.Equal(timerResult.Duration, message.Duration);
            Assert.Equal(timerResult.Offset, message.Offset);
            Assert.Equal(timerResult.StartTime, message.StartTime);
            Assert.Equal(eventName, message.EventName);
            Assert.Equal(eventCategory, message.EventCategory);
            Assert.Equal(eventCategory, message.EventCategory);
            Assert.Equal(executedType, message.ExecutedType);
            Assert.Equal(method, message.ExecutedMethod); 
        }

        [Theory, AutoMock]
        public void BuildDetails(TimerResult timerResult, bool isChildAction, Type executedType, MethodInfo method, string eventName, string eventCategory)
        {
            var message = new ActionBaseMessage(timerResult, isChildAction, executedType, method, eventName, eventCategory);

            var dictionary = new Dictionary<string, object>();
            message.BuildDetails(dictionary);

            Assert.Equal(3, dictionary.Count); 
            Assert.Equal(isChildAction, dictionary["IsChildAction"]);
            Assert.Equal(method.Name, dictionary["ExecutionMethod"]); 
            Assert.Equal(executedType, dictionary["ExecutedType"]);
        }
    }
}