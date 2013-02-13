using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.AspNet.Message;
using Glimpse.AspNet.Model;
using Glimpse.AspNet.SerializationConverter;
using Glimpse.Core.Tab.Assist;
using Xunit;

namespace Glimpse.Test.AspNet.SerializationConverter
{
    public class TraceModelConverterShould
    {
        [Fact]
        public void ConvertToList()
        {
            var model = new List<TraceMessage> { new TraceMessage { Category = FormattingKeywords.Ms, FromFirst = new TimeSpan(12), FromLast = new TimeSpan(23), IndentLevel = 0, Message = "test" } };

            var converter = new ListOfTraceMessageConverter();
            var result = converter.Convert(model) as IEnumerable<object>;

            Assert.NotNull(result);
            Assert.True(result.Count() > 0);
        }
    }
}
