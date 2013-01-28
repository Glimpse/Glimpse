using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Used to describe when a given <see cref="IRuntimePolicy"/> should be executed.
    /// </summary>
    [Flags]
    public enum RuntimeEvent
    {
        /// <summary>
        /// During the Glimpse initialization process.
        /// </summary>
        Initialize = 1,

        /// <summary>
        /// When a Http request is beginning.
        /// </summary>
        BeginRequest = 2,

        /// <summary>
        /// When a Http request has begun to access user session store.
        /// </summary>
        /// <remarks>
        /// All frameworks may not support <c>BeginSessionAccess</c>.
        /// </remarks>
        BeginSessionAccess = 4,

        /// <summary>
        /// When a Glimpse <see cref="IResource"/> is being executed.
        /// </summary>
        ExecuteResource = 8,

        /// <summary>
        /// When a Http request has ended access to the user session store.
        /// </summary>
        /// <remarks>
        /// All frameworks may not support <c>BeginSessionAccess</c>.
        /// </remarks>
        EndSessionAccess = 16,

        /// <summary>
        /// When a Http request has ended.
        /// </summary>
        /// <remarks>
        /// This is the most commonly used <see cref="RuntimeEvent"/>.
        /// </remarks>
        EndRequest = 32
    }
}