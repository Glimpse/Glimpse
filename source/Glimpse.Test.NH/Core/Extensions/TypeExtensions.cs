using System;
using System.Linq;
using NUnit.Framework;

namespace Glimpse.Test.NH.Core.Extensions
{
    public static class TypeExtensions
    {
        public static void ShouldImplement<T>(this Type actual)
        {
            var interfaceType = typeof(T);
            Assert.IsTrue(interfaceType.IsInterface, "T should be an interface");

            var interfaces = actual.GetInterfaces().ToList();
            if (!interfaces.Contains(interfaceType))
            {
                throw new AssertionException(string.Format("{0} does not implement {1}", actual, interfaceType));
            }
        }
    }
}