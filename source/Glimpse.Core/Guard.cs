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
        /// <param name="argumentValue">The argument to check against <code>null</code></param>
        /// <param name="argumentName">The name of the argument used when throwing a <see cref="ArgumentNullException"/></param>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Guards against null or empty values
        /// </summary>
        /// <param name="argumentValue">The argument to check against <code>null</code> or an empty string</param>
        /// <param name="argumentName">The name of the argument used when throwing a <see cref="ArgumentNullException"/></param>
        public static void StringNotNullOrEmpty(string argumentValue, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}