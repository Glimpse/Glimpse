namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// A subtype of <see cref="TimerResult"/> which includes the result of non-void method calls.
    /// </summary>
    /// <typeparam name="T">The type returned by the executed method.</typeparam>
    public class TimerResult<T> : TimerResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public T Result { get; set; }
    }
}