using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client the Html needed to render the Glimpse pop-up window.
    /// </summary>
    public class PopupResource : IPrivilegedResource, IKey
    {
        /// <summary>
        /// Gets the name of the resource.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <remarks>
        /// Resource name's should be unique across a given web application. If multiple <see cref="IResource" /> implementations contain the same name, Glimpse may throw an exception resulting in an Http 500 Server Error.
        /// </remarks>
        public string Name
        {
            get { return "glimpse_popup"; }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key. Only valid JavaScript identifiers should be used for future compatibility.
        /// </value>
        public string Key
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the required or optional parameters that a resource needs as processing input.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.RequestId, ResourceParameter.Hash }; }
        }

        /// <summary>
        /// Executes the specified resource with the specific context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <see cref="IResourceResult" /> that can be executed when the Http response is ready to be returned.
        /// </returns>
        /// <exception cref="System.NotSupportedException">Throws a <see cref="NotSupportedException"/> since this is a <see cref="IPrivilegedResource"/>.</exception>
        public IResourceResult Execute(IResourceContext context)
        {
            throw new NotSupportedException(string.Format(Resources.PrivilegedResourceExecuteNotSupported, GetType().Name));
        }

        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// A <see cref="IResourceResult" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if either <paramref name="context"/> or <paramref name="configuration"/> are <c>null</c>.</exception>
        /// <remarks>
        /// Use of <see cref="IPrivilegedResource" /> is reserved.
        /// </remarks>
        public IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            Guid requestId;
            var request = context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name);

#if NET35
            if (!Glimpse.Core.Backport.Net35Backport.TryParseGuid(request, out requestId))
            {
                return new StatusCodeResourceResult(404, string.Format("Could not parse RequestId of '{0}' as GUID.", request));
            }
#else
            if (!Guid.TryParse(request, out requestId))
            {
                return new StatusCodeResourceResult(404, string.Format("Could not parse RequestId of '{0}' as GUID.", request));
            }
#endif

            var requestStore = configuration.FrameworkProvider.HttpRequestStore;
            var generateScriptTags = requestStore.Get<Func<Guid?, string>>(Constants.ClientScriptsStrategy);

            var scriptTags = generateScriptTags(requestId); 
            var html = string.Format("<!DOCTYPE html><html><head><meta charset='utf-8'><title>Glimpse Popup</title></head><body class='glimpse-popup'>{0}</body></html>", scriptTags);

            return new HtmlResourceResult(html);
        }
    }
}