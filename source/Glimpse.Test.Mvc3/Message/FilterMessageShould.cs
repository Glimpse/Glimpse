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
    public class FilterMessageShould
    {
        [Theory, AutoMock]
        public void Construct(TimerResult timerResult, FilterCategory filterCategory, Type resultType, bool isChildAction, Type executedType, MethodInfo method)
        {
            var message = new FilterMessage(timerResult, filterCategory, resultType, isChildAction, executedType, method);

            Assert.Equal(timerResult.Duration, message.Duration);
            Assert.Equal(timerResult.Offset, message.Offset);
            Assert.Equal(timerResult.StartTime, message.StartTime);  
            Assert.Equal(executedType, message.ExecutedType);
            Assert.Equal(method, message.ExecutedMethod);
            Assert.Equal(filterCategory, message.Category);
            Assert.Equal(resultType, message.ResultType);
            Assert.Equal(string.Format("{0}", filterCategory.ToString()), message.EventName);
        }

        [Theory, AutoMock]
        public void BuildDetails(TimerResult timerResult, FilterCategory filterCategory, Type resultType, bool isChildAction, Type executedType, MethodInfo method, string eventName, string eventCategory)
        {
            var message = new FilterMessage(timerResult, filterCategory, resultType, isChildAction, executedType, method, eventName, eventCategory);

            var dictionary = new Dictionary<string, object>();
            message.BuildDetails(dictionary);

            Assert.Equal(4, dictionary.Count);
            Assert.Equal(resultType, dictionary["ResultType"]);
        }
    }
}