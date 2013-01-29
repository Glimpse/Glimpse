namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for a data store that can be implemented as a storage strategy.
    /// This could be used to store request data in memory, to disk or any other 
    /// source.
    /// </summary>
    /// <remarks>
    /// In the case of storing requests to disk, it would mean that the system
    /// would still work in a multi server environment (i.e. a web farm).
    /// </remarks>
    public interface IDataStore
    {
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value stored at given key.</returns>
        object Get(string key);

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void Set(string key, object value);

        /// <summary>
        /// Determines whether the data store contains a definition for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if it contains the specified key; otherwise, <c>false</c>.</returns>
        bool Contains(string key);
    }
}
