using System;
using Glimpse.Core.SerializationConverter;
using Xunit;

namespace Glimpse.Test.Core.SerializationConverter
{
    public class TimeSpanConverterShould
    {
        [Fact]
        public void HandleNull()
        {
            var converter = new TimeSpanConverter();

            var result = converter.Convert(null);

            Assert.Null(result);
        }

        [Fact]
        public void SupportTimeSpans()
        {
            var converter = new TimeSpanConverter();

            var result = converter.SupportedTypes;

            Assert.Contains(typeof(TimeSpan), result);
            Assert.Contains(typeof(TimeSpan?), result);
        }

        [Fact]
        public void HandleNullableTimeSpans()
        {
            TimeSpan? input = new TimeSpan(12220);

            var converter = new TimeSpanConverter();

            var result = converter.Convert(input);

            Assert.Equal(1.22, result);
        }

        [Fact]
        public void HandleTimeSpans()
        {
            TimeSpan input = new TimeSpan(12220);

            var converter = new TimeSpanConverter();

            var result = converter.Convert(input);

            Assert.Equal(1.22, result);
        }

        [Fact]
        public void HandleNullableTimeSpansWithNoValue()
        {
            TimeSpan? input = null;

            var converter = new TimeSpanConverter();

            var result = converter.Convert(input);

            Assert.Null(result);
        }
    }
}