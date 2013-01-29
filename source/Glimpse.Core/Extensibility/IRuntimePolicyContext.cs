using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the context that is used when a runtime policy 
    /// is being executed.
    /// </summary>
    public interface IRuntimePolicyContext : IContext
    {
        /// <summary>
        /// Gets the request metadata.
        /// </summary>
        /// <value>The request metadata.</value>
        IRequestMetadata RequestMetadata { get; }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <typeparam name="T">Type of result that is expected.</typeparam>
        /// <returns>The request context that is being used.</returns>
        T GetRequestContext<T>() where T : class;
    }
}