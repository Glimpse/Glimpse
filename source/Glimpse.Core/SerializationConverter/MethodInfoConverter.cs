using System.Reflection;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.SerializationConverter
{
    /// <summary>
    /// The <see cref="ISerializationConverter"/> implementation responsible converting <see cref="MethodInfo"/> representation's into a simpler format.
    /// </summary>
    public class MethodInfoConverter : SerializationConverter<MethodInfo>
    {
        /// <summary>
        /// Converts the specified method info.
        /// </summary>
        /// <param name="methodInfo">The method info.</param>
        /// <returns>The method name, without namespace identifiers, with trailing parenthesis.</returns>
        public override object Convert(MethodInfo methodInfo)
        {
            var nameParts = methodInfo.Name.Split('.');
            return nameParts[nameParts.Length - 1] + "()";
        }
    }
}