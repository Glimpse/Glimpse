namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for the base context that is used by various concepts 
    /// in the system. Typically uses in place of passing multiple arguments 
    /// to a method.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        ILogger Logger { get; }
    }
}