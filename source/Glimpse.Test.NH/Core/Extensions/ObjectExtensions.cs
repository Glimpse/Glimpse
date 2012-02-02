using NUnit.Framework;

namespace Glimpse.Test.NH.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static void ShouldBeNull(this object actual)
        {
            Assert.IsNull(actual);
        }

        public static void ShouldNotBeNull<T>(this T actual)
        {
            Assert.IsNotNull(actual);
        }

        public static void ShouldBeEqualTo<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldNotBeEqualTo<T>(this T actual, T expected)
        {
            Assert.AreNotEqual(expected, actual);
        }

        public static void ShouldBeSameAs<T>(this T actual, T expected)
        {
            Assert.AreSame(expected, actual);
        }

        public static void ShouldNotBeSameAs<T>(this T actual, T expected)
        {
            Assert.AreNotSame(expected, actual);
        }

        public static void ShouldBeOfType<T>(this object actual)
        {
            Assert.IsAssignableFrom<T>(actual);
        }
    }
}