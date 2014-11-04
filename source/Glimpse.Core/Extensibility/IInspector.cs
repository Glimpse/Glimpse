using Glimpse.Core.Framework;

namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of an inspector that runs during startup. This provides the means 
    /// by which a Tab can setup any listeners, proxies, etc that are needed to gather the 
    /// data needed the corresponding <see cref="ITab"/>. 
    /// </summary>
    /// <remarks>
    /// This interface can be implemented on the same class as the <see cref="ITab"/>, but 
    /// typically it would be implemented on a different class for separation of concerns.
    /// </remarks>
    public interface IInspector
    {
        /// <summary>
        /// Setups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// Executed during initialization of the <see cref="GlimpseRuntime" />
        /// </remarks>
        void Setup(IInspectorContext context);
    }
}