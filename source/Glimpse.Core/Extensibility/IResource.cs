using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <para>
    /// <c>IResource</c> provides Glimpse clients with data and assets for rendering to the end user.
    /// </para>
    /// <para>
    /// When implemented, a resource will be discovered and added to the collection of resources. 
    /// </para>
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks>Resource name's should be unique across a given web application. If multiple <see cref="IResource"/> implementations contain the same name, Glimpse may throw an exception resulting in an Http 500 Server Error.</remarks>
        string Name { get; }

        /// <summary>
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>The parameters.</value>
        IEnumerable<ResourceParameterMetadata> Parameters { get; }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns><see cref="IResourceResult"/> that can be executed when the Http response is ready to be returned.</returns>
        IResourceResult Execute(IResourceContext context);
    }
}
