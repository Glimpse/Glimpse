namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition for a client script that will be injected into response payloads.
    /// </summary>
    public interface IClientScript
    {
        /// <summary>
        /// Gets the order in which the script should be injected into response payloads.
        /// </summary>
        /// <value>The order.</value>
        ScriptOrder Order { get; }
    }
}