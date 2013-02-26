namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// <c>IWrapper&lt;T&gt;</c> provides a common way to leverage the <see href="http://en.wikipedia.org/wiki/Decorator_pattern">decorator pattern</see>.
    /// </summary>
    /// <typeparam name="T">Type of target object.</typeparam>
    public interface IWrapper<T>
    {
        /// <summary>
        /// Gets the wrapped target object.
        /// </summary>
        /// <returns>The underlying target object.</returns>
        T GetWrappedObject();
    }
}