using System;
using NUnit.Framework;

namespace Glimpse.Test.NH.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ShouldHaveMessage(this Exception exception, string message)
        {
            Assert.AreEqual(message, exception.Message);
        }
    }
}