using System;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Contains event data for <see cref="ActiveGlimpseRequestContexts"/> related events
    /// </summary>
    public class ActiveGlimpseRequestContextEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveGlimpseRequestContextEventArgs" />
        /// </summary>
        /// <param name="glimpseRequestId">The Id assigned to the request by Glimpse.</param>
        public ActiveGlimpseRequestContextEventArgs(Guid glimpseRequestId)
        {
            GlimpseRequestId = glimpseRequestId;
            RaisedOn = DateTime.Now;
        }

        /// <summary>
        /// Gets the Glimpse Id assigned to this request
        /// </summary>
        public Guid GlimpseRequestId { get; private set; }

        /// <summary>
        /// Gets the moment when the event was raised
        /// </summary>
        public DateTime RaisedOn { get; private set; }
    }
}