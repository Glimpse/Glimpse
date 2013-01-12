using System.Collections.Generic;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of a resource that will be exposed by the system as a http endpoint.
    /// These endpoints typically return content of varying types (i.e. html, images,
    /// JavaScript, etc).
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Gets the parameters that the resource is expecting to work with.
        /// </summary>
        /// <value>The parameters.</value>
        IEnumerable<ResourceParameterMetadata> Parameters { get; }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Resource that can be executed when the response is ready to be returned.</returns>
        IResourceResult Execute(IResourceContext context);
    }
}
