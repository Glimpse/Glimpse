using System;
using NUnit.Framework;

namespace Glimpse.Test.NH.Core.Extensions
{
    public static class ExceptionExtensionsArgumentNullException
    {
        public static void ShouldHaveParamName(this ArgumentNullException exception, string paramName)
        {
            Assert.AreEqual(paramName, exception.ParamName);
        }
    }
}