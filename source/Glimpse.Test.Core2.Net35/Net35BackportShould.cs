using System;
using Glimpse.Core2;
using Glimpse.Core2.Backport;
using Xunit;

namespace Glimpse.Test.Core2.Net35
{
    public class Net35BackportShould
    {
        [Fact]
        public void ParseCorrectGuidStrings()
        {
            var guid = Guid.NewGuid();
            Guid output;

            Assert.True(Net35Backport.TryParseGuid(guid.ToString(), out output));
            Assert.Equal(guid, output);
        }

        [Fact]
        public void NotParseIncorrectGuidStrings()
        {
            var guid = "crap";
            Guid output;

            Assert.False(Net35Backport.TryParseGuid(guid, out output));
            Assert.Equal(default(Guid), output);
        }

        [Fact]
        public void ReturnTrueIfFlagIsContained()
        {
            var input = RuntimePolicy.On;

            Assert.True(input.HasFlag(RuntimePolicy.ModifyResponseHeaders));
        }

        [Fact]
        public void ReturnFalseIfFlagIsNotContained()
        {
            var input = RuntimePolicy.Off;

            Assert.False(input.HasFlag(RuntimePolicy.ModifyResponseHeaders));
        }

        [Fact]
        public void ParseCorrectEnumValue()
        {
            RuntimePolicy result;

            Assert.True(Net35Backport.TryParseEnum("on", true, out result));
            Assert.Equal(RuntimePolicy.On, result);
        }


        [Fact]
        public void NotParseIncorrectEnumValue()
        {
            RuntimePolicy result;

            Assert.False(Net35Backport.TryParseEnum("bad string", true, out result));
            Assert.Equal(default(RuntimePolicy), result);
        }
    }
}