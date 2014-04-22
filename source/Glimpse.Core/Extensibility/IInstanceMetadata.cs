using Glimpse.Core.Configuration;
using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Provides the ability for metadata to be sliced into the metadata
    /// response for a given instance of a request.
    /// </summary>
    public interface IInstanceMetadata
    {
        /// <summary>
        /// Gets the key that should be used in the serialized output
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the instance metadata for this strategy
        /// </summary>
        /// <param name="configuration">Current configuration that the system has.</param>
        /// <param name="requestContext">Context of the current request.</param>
        /// <returns>The metadata to be used for the given key</returns>
        object GetInstanceMetadata(IConfiguration configuration, IGlimpseRequestContext requestContext);
    }
}