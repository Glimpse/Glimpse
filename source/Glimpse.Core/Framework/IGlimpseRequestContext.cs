using System;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Represents the context of a specific request, which is used as an access point to the request's <see cref="IRequestResponseAdapter"/> handle
    /// </summary>
    public interface IGlimpseRequestContext
    {
        /// <summary>
        /// Gets the Glimpse Id assigned to the referenced request
        /// </summary>
        Guid GlimpseRequestId { get; }

        /// <summary>
        /// Gets the <see cref="IRequestResponseAdapter"/> for the referenced request
        /// </summary>
        IRequestResponseAdapter RequestResponseAdapter { get; }

        /// <summary>
        /// Gets the <see cref="IDataStore"/> for the referenced request
        /// </summary>
        IDataStore RequestStore { get; }

        /// <summary>
        /// Gets or sets the active <see cref="RuntimePolicy"/> for the referenced request
        /// </summary>
        RuntimePolicy CurrentRuntimePolicy { get; set; }

        /// <summary>
        /// Gets the <see cref="RequestHandlingMode"/> for the referenced request
        /// </summary>
        RequestHandlingMode RequestHandlingMode { get; }

        /// <summary>
        /// Starts timing the execution of the referenced request
        /// </summary>
        void StartTiming();

        /// <summary>
        /// Gets the <see cref="IExecutionTimer"/> for the referenced request
        /// </summary>
        IExecutionTimer CurrentExecutionTimer { get; }

        /// <summary>
        /// Stops timing the execution of the referenced request
        /// </summary>
        /// <returns>The elapsed time since the start of the timing</returns>
        TimeSpan StopTiming();
    }
}