namespace Glimpse.Core.Extensibility
{
    /// <summary>
    /// Definition of a pipeline inspector that runs during startup. This provides the means 
    /// by which a Tab can setup any listeners, proxies, etc that are needed to gather the 
    /// data needed the corresponding <see cref="ITab"/>. 
    /// </summary>
    /// <remarks>
    /// This interface can be implemented on the same class as the <see cref="ITab"/>, but 
    /// typically it would be implemented on a different class for separation of concerns.
    /// </remarks>
    public interface IPipelineInspector
    {
        /// <summary>
        /// Setups the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>
        /// Executed during the <see cref="Glimpse.Core.Framework.IGlimpseRuntime.Initialize"/> phase of 
        /// system startup. Specifically, with the ASP.NET provider, this is wired to/implemented by the 
        /// <see cref="System.Web.IHttpModule.Init"/> method.
        /// </remarks>
        void Setup(IPipelineInspectorContext context);
    }
}
