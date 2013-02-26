using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Glimpse.Core.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Core.Message
{
    public class ExecutionMessageShould
    {
        [Theory, AutoMock]
        public void ShouldBeAbleToBuildWithFactory(Type executedType, MethodInfo executedMethod)
        {
            var testMessage = new TestMessage().AsSourceMessage(executedType, executedMethod);

            Assert.Equal(executedType, testMessage.ExecutedType);
            Assert.Equal(executedMethod, testMessage.ExecutedMethod); 
        }

        public class TestMessage : ISourceMessage
        {
            public Guid Id { get; private set; }   
            public Type ExecutedType { get; set; }
            public MethodInfo ExecutedMethod { get; set; }
        }
    }
}
