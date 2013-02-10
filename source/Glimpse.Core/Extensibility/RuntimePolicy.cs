using System;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Used to describe what operations Glimpse is allowed to perform during a Http request/response.
    /// </summary>
    [Flags]
    public enum RuntimePolicy
    {
        /// <summary>
        /// <c>Off</c> allows Glimpse to perform any operations on a Http request/response.
        /// </summary>
        /// <remarks>
        /// When a request's runtime policy is off, Glimpse will not modify any part of the response or capture any data.
        /// </remarks>
        Off = 1,

        /// <summary>
        /// <c>ExecuteResourceOnly</c> allows Glimpse to only respond to <see cref="IResource"/> requests. This mode is effectively a special type of <c>Off</c>.
        /// </summary>
        ExecuteResourceOnly = 2 | Off,

        /// <summary>
        /// <c>PersistResults</c> allows Glimpse to write request metadata to current <see cref="IPersistenceStore"/> instance.
        /// </summary>
        /// <remarks>
        /// The act of persisting request metadata does not alter an Http response in any way.
        /// </remarks>
        PersistResults = 4,

        /// <summary>
        /// <c>ModifyResponseHeaders</c> allows Glimpse to write custom Http headers and set cookies on the Http response.
        /// </summary>
        /// <remarks>
        /// <c>ModifyResponseHeaders</c> also grants Glimpse the ability to <c>PersistResults</c>.
        /// </remarks>
        ModifyResponseHeaders = 8 | PersistResults,

        /// <summary>
        /// <c>ModifyResponseBody</c> allows Glimpse to write to the Http response body.
        /// </summary>
        /// <remarks>
        /// <c>ModifyResponseBody</c> also grants Glimpse the ability to <c>PersistResults</c> and <c>ModifyResponseHeaders</c>.
        /// </remarks>
        ModifyResponseBody = 16 | ModifyResponseHeaders,

        /// <summary>
        /// <c>DisplayGlimpseClient</c> allows Glimpse to write the Glimpse JavaScript client <c>&lt;script&gt;</c> tag to the Http response body.
        /// </summary>
        /// <remarks>
        /// <c>DisplayGlimpseClient</c> also grants Glimpse the ability to <c>PersistResults</c>, <c>ModifyResponseHeaders</c> and <c>ModifyResponseBody</c>.
        /// </remarks>
        DisplayGlimpseClient = 32 | ModifyResponseBody,

        /// <summary>
        /// <c>On</c> allows Glimpse to run all operations against an Http request/response.
        /// </summary>
        /// <remarks>
        /// <c>On</c> also grants Glimpse the ability to <c>PersistResults</c>, <c>ModifyResponseHeaders</c>, <c>ModifyResponseBody</c> and <c>DisplayGlimpseClient</c>.
        /// </remarks>
        On = DisplayGlimpseClient
    }
}