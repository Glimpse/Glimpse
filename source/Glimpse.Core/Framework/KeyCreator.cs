using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Creates keys depending on the input given
    /// </summary>
    internal static class KeyCreator
    {
        /// <summary>
        /// Creates a key based on the given <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The input to base a key on</param>
        /// <returns>The key</returns>
        public static string Create(object obj)
        {
            Guard.ArgumentNotNull(obj, "obj");

            var keyProvider = obj as IKey;

            string result = keyProvider != null ? keyProvider.Key : obj.GetType().FullName;

            return result.Replace('.', '_').Replace(' ', '_').ToLower();
        }
    }
}