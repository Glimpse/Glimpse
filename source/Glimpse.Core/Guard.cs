using System;

namespace Glimpse.Core
{
    /// <summary>
    /// Guard class, used for guard clauses and argument validation
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Guards against null values
        /// </summary>
        /// <param name="argumentName">The name of the argument used when throwing a <see cref="ArgumentNullException"/></param>
        /// <param name="argumentValue">The argument to check against <code>null</code></param>
        public static void ArgumentNotNull(string argumentName, object argumentValue)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}