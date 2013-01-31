using System;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Defines methods to implement the Glimpse runtime
    /// </summary>
    public interface IGlimpseRuntime
    {
        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value><c>true</c> if this instance is initialized; otherwise, <c>false</c>.</value>
        bool IsInitialized { get; }

        /// <summary>
        /// Begins the request.
        /// </summary>
        /// <remarks>
        /// Called when ever the implementing framework registers a request start. Specifically, 
        /// with the ASP.NET provider, this is wired to the 
        /// <see cref="System.Web.HttpApplication.BeginRequest"/> method.
        /// </remarks>
        void BeginRequest();

        /// <summary>
        /// Ends the request.
        /// </summary>
        /// <remarks>
        /// Called when ever the implementing framework registers a request end. Specifically, 
        /// with the ASP.NET provider, this is wired to the 
        /// <see cref="System.Web.HttpApplication.PostReleaseRequestState"/> method.
        /// </remarks>
        void EndRequest();

        /// <summary>
        /// Executes the default resource.
        /// </summary>
        /// <remarks>
        /// Specifically, with the ASP.NET provider, this is wired to the 
        /// <see cref="System.Web.IHttpHandler.ProcessRequest"/> method.
        /// <seealso cref="Glimpse.Core.Extensibility.IResourceResult"/>
        /// <seealso cref="Glimpse.Core.Extensibility.IResource"/>
        /// </remarks>
        void ExecuteDefaultResource();

        /// <summary>
        /// Executes the resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="parameters">The parameters.</param>
        /// <remarks>
        /// Specifically, with the ASP.NET provider, this is wired to the 
        /// <see cref="System.Web.IHttpHandler.ProcessRequest"/> method.
        /// <seealso cref="Glimpse.Core.Extensibility.IResourceResult"/>
        /// <seealso cref="Glimpse.Core.Extensibility.IResource"/>
        /// </remarks>
        void ExecuteResource(string resourceName, ResourceParameters parameters);

        /// <summary>
        /// Begins the session access.
        /// </summary>
        /// <remarks>
        /// Called when ever the implementing framework registers a clients session start. Code that is 
        /// executed off this methods should have access to the session state store. Specifically, 
        /// with the ASP.NET provider, this is wired to the 
        /// <see cref="System.Web.HttpApplication.PostAcquireRequestState"/> method.
        /// </remarks>
        void BeginSessionAccess();

        /// <summary>
        /// Ends the session access.
        /// </summary>
        /// <remarks>
        /// Called when ever the implementing framework registers a clients session end. Code that is 
        /// executed off this methods should still have access to the session state store. Specifically, 
        /// with the ASP.NET provider, this is wired to the 
        /// <see cref="System.Web.HttpApplication.PostRequestHandlerExecute"/> method.
        /// </remarks>
        void EndSessionAccess();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns><c>true</c> if system initialized successfully, <c>false</c> otherwise</returns>
        /// <remarks>
        /// Typically used to wire up framework events to the corresponding runtime methods. Depending
        /// on framework implementation, this could be called multiple times per "application pool" 
        /// recycle. Specifically, with the ASP.NET provider, this is wired to/implemented by the 
        /// <see cref="System.Web.IHttpModule.Init"/> method.
        /// </remarks>
        bool Initialize();

        /// <summary>
        /// Generates the script tags.
        /// </summary>
        /// <param name="requestId">The request id.</param>
        /// <returns>Generated script tag.</returns>
        //string GenerateScriptTags(Guid requestId);
    }
}
