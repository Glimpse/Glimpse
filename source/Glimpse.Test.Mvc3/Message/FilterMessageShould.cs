using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Glimpse.Mvc.Message;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.Mvc3.Message
{
    public class FilterMessageShould
    {
        [Theory, AutoMock]
        public void ShouldBeAbleToBuildWithFactory(FilterCategory category, Type resultType)
        {
            var testMessage = new TestMessage().AsFilterMessage(category, resultType);

            Assert.Equal(category, testMessage.Category);
            Assert.Equal(resultType, testMessage.ResultType);
        }

        public class TestMessage : IFilterMessage
        {
            public Guid Id { get; private set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public FilterCategory Category { get; set; }
            public Type ResultType { get; set; }
        }
    }
}
