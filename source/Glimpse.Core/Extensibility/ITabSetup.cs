namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition which allows a tab to inject any hooks it needs within the system.
    /// </summary>
    /// <remarks>
    /// If this interface isn't implemented on a <see cref="ITab"/> no documentation 
    /// will be provided.
    /// </remarks>
    public interface ITabSetup
    {
        /// <summary>
        /// Setups the targeted tab using the specified context.
        /// </summary>
        /// <param name="context">The context which should be used.</param>
        void Setup(ITabSetupContext context);
    }
}
