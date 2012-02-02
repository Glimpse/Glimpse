using NUnit.Framework;

namespace Glimpse.Test.NH.Core.Extensions
{
    public static class BoolExtensions
    {
        public static void ShouldBeTrue(this bool value)
        {
            Assert.AreEqual(true, value);
        }

        public static void ShouldBeFalse(this bool value)
        {
            Assert.AreEqual(false, value);
        }
    }
}