namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Allows a <see cref="ITab"/> to define what key it should use in the JSON payload.
    /// </summary>
    /// <remarks>
    /// If this interface isn't implemented on a <see cref="ITab"/> a key will be automatically 
    /// generated instead. Typically used in cases where a client tab is needing to know the key
    /// value so that it can perform data lookups.
    /// </remarks>
    public interface IKey
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        string Key { get; }
    }
}