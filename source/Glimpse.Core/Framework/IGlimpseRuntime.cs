using System;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to implement the Glimpse runtime
    /// </summary>
    public interface IGlimpseRuntime : IDisposable
    {
        /// <summary>
        /// Calling this method will allow Glimpse to decide to hook into the given request or not
        /// </summary>
        /// <param name="requestResponseAdapter">The <see cref="IRequestResponseAdapter"/></param>
        /// <returns>A <see cref="GlimpseRequestContextHandle"/> for the given request which also indicates how Glimpse is actually handling that request.</returns>
        GlimpseRequestContextHandle BeginRequest(IRequestResponseAdapter requestResponseAdapter);

        /// <summary>
        /// Calling this method indicates Glimpse to finalize processing the request referenced by the given <paramref name="glimpseRequestContextHandle"/>"/>
        /// </summary>
        /// <param name="glimpseRequestContextHandle">The Glimpse handle of the corresponding request</param>
        void EndRequest(GlimpseRequestContextHandle glimpseRequestContextHandle);

        /// <summary>
        /// Executes the given resource.
        /// </summary>
        /// <param name="glimpseRequestContextHandle">The Glimpse handle of the corresponding request</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="parameters">The parameters.</param>
        void ExecuteResource(GlimpseRequestContextHandle glimpseRequestContextHandle, string resourceName, ResourceParameters parameters);

#warning CGI: These methods should be replaced with a CustomEvent method, passing in the custom event name as a string, so additional events can be added
        /// <summary>
        /// Begins the session access.
        /// </summary>
        /// <remarks>
        /// Called when ever the implementing framework registers a clients session start. Code that is 
        /// executed off this methods should have access to the session state store. Specifically, 
        /// with the ASP.NET provider, this is wired to the <c>PostAcquireRequestState</c> method.
        /// </remarks>
        void BeginSessionAccess(GlimpseRequestContextHandle glimpseRequestContextHandle);

        /// <summary>
        /// Ends the session access.
        /// </summary>
        /// <remarks>
        /// Called when ever the implementing framework registers a clients session end. Code that is 
        /// executed off this methods should still have access to the session state store. Specifically, 
        /// with the ASP.NET provider, this is wired to the <c>PostRequestHandlerExecute</c> method.
        /// </remarks>
        void EndSessionAccess(GlimpseRequestContextHandle glimpseRequestContextHandle);

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        IReadOnlyConfiguration Configuration { get; }

        /// <summary>
        /// Returns the corresponding <see cref="IGlimpseRequestContext"/> for the given <paramref name="glimpseRequestId"/>
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse request Id</param>
        /// <param name="glimpseRequestContext">The corresponding <see cref="IGlimpseRequestContext"/></param>
        /// <returns>Boolean indicating whether the corresponding <see cref="IGlimpseRequestContext"/> was found.</returns>
        bool TryGetRequestContext(Guid glimpseRequestId, out IGlimpseRequestContext glimpseRequestContext);

        /// <summary>
        /// Returns the <see cref="IGlimpseRequestContext"/> corresponding to the current request.
        /// </summary>
        IGlimpseRequestContext CurrentRequestContext { get; }

        string GenerateScriptTags(IGlimpseRequestContext glimpseRequestContext);

        string GenerateScriptTags(GlimpseRequestContextHandle glimpseRequestContextHandle);
    }
}
