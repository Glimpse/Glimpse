using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Used to describe when a given policy should run.
    /// </summary>
    /// <remarks>
    /// This is used at a request by request level.
    /// </remarks>
    [Flags]
    public enum RuntimeEvent
    {
        /// <summary>
        /// During the initialization process.
        /// </summary>
        Initialize = 1,

        /// <summary>
        /// When the request is beginning.
        /// </summary>
        BeginRequest = 2,

        /// <summary>
        /// When the request has begun to access the session.
        /// </summary>
        BeginSessionAccess = 4,

        /// <summary>
        /// When the resource itself is being executed.
        /// </summary>
        ExecuteResource = 8,

        /// <summary>
        /// When the request has ended access to the session.
        /// </summary>
        EndSessionAccess = 16,

        /// <summary>
        /// When the request has ended.
        /// </summary>
        EndRequest = 32
    }
}