using System;

namespace Glimpse.Test.NH.Core.Extensions
{
    public static class ActionExtensions
    {
        public static TExc ShouldThrow<TExc>(this Delegate actual) 
            where TExc : Exception
        {
            try
            {
                actual.DynamicInvoke();
            }
            catch (Exception e)
            {
                if (e.InnerException is TExc)
                {
                    return (TExc)e.InnerException;
                }

                throw new Exception(string.Format("Exception {0} expected, but was : {1}", typeof(TExc).Name, e.InnerException));
            }

            throw new Exception(string.Format("Exception expected but was not thrown: {0}", typeof(TExc).Name));
        }

        public static void ShouldNotThrow(this Delegate actual)
        {
            actual.DynamicInvoke();
        }
    }
}