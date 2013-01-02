namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for an alternate that should replace a given Type
    /// </summary>
    public interface IAlternateType<T>
    {
        /// <summary>
        /// Tries to create an alternate for the given target.
        /// </summary>
        /// <param name="originalObj">The original obj.</param>
        /// <param name="newObj">The new obj.</param>
        /// <returns><c>true</c> if the create was successful, <c>false</c> otherwise</returns>
        bool TryCreate(T originalObj, out T newObj);
    }
}