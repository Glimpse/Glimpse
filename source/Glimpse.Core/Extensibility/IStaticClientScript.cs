namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for a static client script that will be injected into response payloads.
    /// TODO: Provide a config only way to create static client scripts without implementing a class
    /// </summary>
    /// <remarks>
    /// This interface only returns URIs that will be included in the payload. It doesn't 
    /// detail with any raw content in these files.
    /// </remarks>
    public interface IStaticClientScript : IClientScript
    {
        /// <summary>
        /// Gets the URI for where the script is registered.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The URI where the asset lives.</returns>
        string GetUri(string version);
    }
}