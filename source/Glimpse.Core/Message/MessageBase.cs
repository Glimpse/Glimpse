using System;

namespace Glimpse.Core.Message
{
    /// <summary>
    /// Core message that can be used to build other messages.
    /// </summary>
    public class MessageBase : IMessage
    {
        /// <summary>
        /// Constructs message.
        /// </summary>
        public MessageBase() 
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Constructs message with a defined message id.
        /// </summary>
        /// <param name="id"></param>
        public MessageBase(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the id of the message.
        /// </summary>
        /// <value>The id.</value>
        public Guid Id { get; protected internal set; }
    }
}