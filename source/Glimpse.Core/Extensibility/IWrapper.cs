namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for a wrapper that can be used to access the target objects when 
    /// alternatives/proxies are being used.
    /// </summary>
    /// <typeparam name="T">Type of target object.</typeparam>
    public interface IWrapper<T>
    {
        /// <summary>
        /// Gets the target that object is wrapping.
        /// </summary>
        /// <returns>The underlying target object.</returns>
        T GetWrappedObject();
    }
}