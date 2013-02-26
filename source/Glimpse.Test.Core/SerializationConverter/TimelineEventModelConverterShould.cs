using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Model;
using Glimpse.Core.SerializationConverter;
using Glimpse.Core.Tab.Assist;
using Glimpse.Test.Common;
using Xunit;
using Xunit.Extensions;

namespace Glimpse.Test.AspNet.SerializationConverter
{
    public class TimelineEventModelConverterShould
    {
        [Theory, AutoMock]
        public void ConvertToList(TimelineEventModel model)
        {
            var converter = new TimelineEventModelConverter();
            var result = converter.Convert(model);

            Assert.NotNull(result); 
        }
    }
}
