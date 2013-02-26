namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for an alternate that should replace a given Type
    /// </summary>
    /// <typeparam name="T">The type to retrieve and alternate for.</typeparam>
    public interface IAlternateType<T>
    {
        /// <summary>
        /// Tries to create an alternate for the given target.
        /// </summary>
        /// <param name="originalObj">The original object.</param>
        /// <param name="newObj">The new object.</param>
        /// <returns><c>true</c> if the create was successful, <c>false</c> otherwise</returns>
        bool TryCreate(T originalObj, out T newObj);
    }
}