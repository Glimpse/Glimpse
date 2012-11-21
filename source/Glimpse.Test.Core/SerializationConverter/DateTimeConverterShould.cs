using System;
using Glimpse.Core.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Core.SerializationConverter
{
    public class DateTimeConverterShould
    {
        [Fact]
        public void HandleNull()
        {
            var converter = new DateTimeConverter();

            var result = converter.Convert(null);

            Assert.Null(result);
        }

        [Fact]
        public void SupportDateTimes()
        {
            var converter = new DateTimeConverter();

            var result = converter.SupportedTypes;

            Assert.Contains(typeof(DateTime), result);
            Assert.Contains(typeof(DateTime?), result);
        }

        [Fact]
        public void HandleNullableDateTimes()
        {
            DateTime? input = new DateTime(2000,1,1);

            var converter = new DateTimeConverter();

            var result = converter.Convert(input);

            Assert.Equal("01/01/2000 00:00:00", result);
        }

        [Fact]
        public void HandleDateTimes()
        {
            DateTime input = new DateTime(2000, 1, 1);

            var converter = new DateTimeConverter();

            var result = converter.Convert(input);

            Assert.Equal("01/01/2000 00:00:00", result);
        }

        [Fact]
        public void HandleNullableDateTimesWithNoValue()
        {
            DateTime? input = null;

            var converter = new DateTimeConverter();

            var result = converter.Convert(input);

            Assert.Null(result);
        }
    }
}