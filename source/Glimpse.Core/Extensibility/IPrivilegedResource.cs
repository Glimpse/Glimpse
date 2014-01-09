using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <para>
    /// <c>IPrivilegedResource</c> has the ability to directly access Glimpse internals in order to provide Glimpse clients with data and assets for rendering to the end user.
    /// </para>
    /// <para>
    /// When implemented, a resource will be discovered and added to the collection of resources. 
    /// </para>
    /// </summary>
    internal interface IPrivilegedResource : IResource
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        /// <remarks>
        /// Use of <see cref="IPrivilegedResource"/> is reserved.
        /// </remarks>
        /// <returns>
        /// A <see cref="IResourceResult"/>.
        /// </returns>
        IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration, IRequestResponseAdapter requestResponseAdapter); 
    }
}