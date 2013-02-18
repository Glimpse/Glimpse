using System;
using System.Reflection;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// The definition of a simple message, which contains unique Id.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        Guid Id { get; } 
    }
}