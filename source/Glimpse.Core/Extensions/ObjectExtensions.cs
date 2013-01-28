using System;

namespace Glimpse.Core.Extensions
{
    /// <summary>
    /// Extension methods to simplify common tasks completed with <see cref="Object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns the value of <c>ToString</c> if <param name="value">value</param> is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of <c>ToString</c> if <param name="value">value</param> is not null.</returns>
        public static string ToStringOrDefault(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }

        /// <summary>
        /// Returns the value of <c>GetType</c> if <param name="value">value</param> is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of <c>GetType</c> if <param name="value">value</param> is not null.</returns>
        public static Type GetTypeOrNull(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.GetType();
        }
    }
}
