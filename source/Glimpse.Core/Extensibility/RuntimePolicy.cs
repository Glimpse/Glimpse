using System;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Used to describe what activity level Glimpse is allowed to operate in.
    /// </summary>
    /// <remarks>
    /// This is used at a request by request level.
    /// </remarks>
    [Flags]
    public enum RuntimePolicy
    {
        /// <summary>
        /// The off
        /// </summary>
        /// <remarks>
        /// Will not modify any part of the request
        /// </remarks>
        Off = 1,

        /// <summary>
        /// The execute resource only
        /// </summary>
        /// <remarks>
        /// Will only resource endpoints to execute
        /// </remarks>
        ExecuteResourceOnly = 2 | Off,

        /// <summary>
        /// The persist results
        /// </summary>
        /// <remarks>
        /// Will allow results to be persisted to the data store
        /// </remarks>
        PersistResults = 4,

        /// <summary>
        /// The modify response headers
        /// </summary>
        /// <remarks>
        /// Allows the modification of response headers
        /// </remarks>
        ModifyResponseHeaders = 8 | PersistResults,

        /// <summary>
        /// The modify response body
        /// </summary>
        /// <remarks>
        /// Allows the modification of response body
        /// </remarks>
        ModifyResponseBody = 16 | ModifyResponseHeaders,

        /// <summary>
        /// The display glimpse client
        /// </summary>
        /// <remarks>
        /// Whether the client should show in the client browser
        /// </remarks>
        DisplayGlimpseClient = 32 | ModifyResponseBody,

        /// <summary>
        /// The on
        /// </summary>
        /// <remarks>
        /// Everything is turned on
        /// </remarks>
        On = DisplayGlimpseClient
    }
}