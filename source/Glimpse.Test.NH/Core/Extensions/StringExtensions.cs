using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Glimpse.Test.NH.Core.Extensions
{
    public static class StringExtensions
    {
        public static void ShouldStartWith(this string actual, string expected)
        {
            Assert.That(actual, new StartsWithConstraint(expected));
        }

        public static void ShouldContain(this string actual, string expected)
        {
            Assert.That(actual, new ContainsConstraint(expected));
        }
    }
}