namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IStaticClientScript</c>'s are a special type of <see cref="IClientScript"/> that sets the <c>&lt;script&gt;</c> tag's <c>src</c> attribute to a given uri.
    /// </summary>
    /// <remarks>
    /// It is up to the implementer of <c>IStaticClientScript</c> to host a given resource. 
    /// </remarks>
    public interface IStaticClientScript : IClientScript // TODO: Provide a config only way to create static client scripts without implementing a class
    {
        /// <summary>
        /// Gets the Uri to set as the value for the <c>src</c> attribute.
        /// </summary>
        /// <remarks>
        /// The returned Uri should only point to JavaScript files since the <c>&lt;script&gt;</c> tag will be rendered with a <c>type='text/javascript'</c> attribute.
        /// </remarks>
        /// <param name="version">
        /// A unique string that should be combined with the Uri to allow for Http cache invalidation.
        /// <example>
        /// In this example, the version parameter is appended to a query string argument.
        /// <code>
        /// public string GetUri(string version)
        /// {
        ///     return "//localhost/scripts/myScript.js?v=" + version;
        /// }
        /// </code>
        /// </example>
        /// </param>
        /// <returns>A string Uri pointing to a JavaScript asset.</returns>
        string GetUri(string version);
    }
}