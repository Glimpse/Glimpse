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
    public class BoundedFilterMessageShould
    {
        [Theory, AutoMock]
        public void ShouldBeAbleToBuildWithFactory(FilterBounds bounds)
        {
            var testMessage = new TestMessage().AsBoundedFilterMessage(bounds);

            Assert.Equal(bounds, testMessage.Bounds); 
        }

        public class TestMessage : IBoundedFilterMessage
        {
            public Guid Id { get; private set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public FilterBounds Bounds { get; set; }
            public FilterCategory Category { get; set; }
            public Type ResultType { get; set; }
        }
    }
}
