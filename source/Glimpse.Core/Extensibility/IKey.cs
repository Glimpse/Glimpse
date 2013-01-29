namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IKey</c> provides implementers a means to override the automatically generated key for an object.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <c>IKey</c> is used by <see cref="ITab"/> and <see cref="IResource"/> implementations that need consistent and predictable
    /// key names for client side interaction.
    /// </para>
    /// <para>
    /// All keys, rather dynamically generated or specified with <see cref="IKey"/> have spaces (<c>' '</c>) and periods (<c>'.'</c>) removed, 
    /// and are converted to lower case for consistency and simplification of JavaScript access.
    /// </para>
    /// </remarks>
    public interface IKey
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key. Only valid JavaScript identifiers should be used for future compatibility.</value>
        string Key { get; }
    }
}