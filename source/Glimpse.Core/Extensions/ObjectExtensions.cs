using System;

namespace Glimpse.Core.Extensions
{
    /// <summary>
    /// Extension methods to simplify common tasks completed with <see cref="Object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns the value of <c>ToString</c> if <paramref name="value"/> is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of <c>ToString</c> if <paramref name="value"/> is not null.</returns>
        public static string ToStringOrDefault(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }

        /// <summary>
        /// Returns the value of <c>GetType</c> if <paramref name="value"/> is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of <c>GetType</c> if <paramref name="value"/> is not null.</returns>
        public static Type GetTypeOrNull(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.GetType();
        }

        /// <summary>
        /// Casts the value to Type of T if it is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of cast is not null.</returns>
        public static T CastOrDefault<T>(this object value)
        {
            if (value == null)
            {
                return default(T);
            }

            return (T)value;
        }
    }
}
