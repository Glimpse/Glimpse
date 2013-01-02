using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of a resource that has access to higher privileges than a 
    /// normal resource.
    /// </summary>
    internal interface IPrivilegedResource : IResource
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>IResourceResult.</returns>
        /// <remarks>
        /// Normal resources shouldn't have access to the <see cref="IGlimpseConfiguration"/>
        /// </remarks>
        IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration); 
    }
}