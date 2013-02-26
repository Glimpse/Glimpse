using System; 
using System.Reflection; 

namespace Glimpse.Core.Message
{
    /// <summary>
    /// The definition of a message which is published from a proxy implementation.
    /// </summary>
    public interface ISourceMessage
    {
        /// <summary>
        /// Gets or sets the type of the original executed object.
        /// </summary>
        /// <value>
        /// The type of the executed.
        /// </value>
        Type ExecutedType { get; set; }

        /// <summary>
        /// Gets or sets the original executed method.
        /// </summary>
        /// <value>
        /// The executed method.
        /// </value>
        MethodInfo ExecutedMethod { get; set; }
    }

    /// <summary>
    /// Extension methods for populating <see cref="ISourceMessage"/> instances.
    /// </summary>
    public static class SourceMessageExtension
    {
        /// <summary>
        /// Populates relevant properties on the source message.
        /// </summary>
        /// <typeparam name="T">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="executedType">Type of the executed.</param>
        /// <param name="executedMethod">The executed method.</param>
        /// <returns>The <paramref name="message"/> with populated <see cref="ISourceMessage"/> properties.</returns>
        public static T AsSourceMessage<T>(this T message, Type executedType, MethodInfo executedMethod)
            where T : ISourceMessage
        {
            message.ExecutedType = executedType;
            message.ExecutedMethod = executedMethod; 

            return message;
        }
    }
}
