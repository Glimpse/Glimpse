using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    /// <summary>
    /// The <see cref="IResource"/> implementation responsible for providing the Glimpse client the Html needed to render the Glimpse pop-up window.
    /// </summary>
    public class PopupRedirectResource : IPrivilegedResource, IKey
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
            get { return "glimpse_redirect_popup"; }
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
            get { return new[] { ResourceParameter.Hash }; }
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
        /// <param name="requestResponseAdapter">The request response adapter</param>
        /// <returns>
        /// A <see cref="IResourceResult" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Exception thrown if either <paramref name="context"/> or <paramref name="configuration"/> are <c>null</c>.</exception>
        /// <remarks>
        /// Use of <see cref="IPrivilegedResource" /> is reserved.
        /// </remarks>
        public IResourceResult Execute(IResourceContext context, IReadonlyConfiguration configuration, IRequestResponseAdapter requestResponseAdapter)
        {
            //TODO: Can't assume this is here 
            var request = context.PersistenceStore.GetTop(1).FirstOrDefault(); 
            if (request == null)
                return new HtmlResourceResult("<html><body><h1>Sorry no requests are currently available</h1>This is a current limitation of this feature. For Glimpse client to work, Glimpse has to detect at least one requesting for which it is enabled.<br /><br />If you are using this feature and it is causing you issues, please let us know.</body></html>");

            var popupResource = configuration.Resources.FirstOrDefault(r => r.Name.Equals("glimpse_popup", StringComparison.InvariantCultureIgnoreCase));
            var popupUriTemplate = configuration.ResourceEndpoint.GenerateUriTemplate(popupResource, configuration.EndpointBaseUri, configuration.Logger);

            return new CacheControlDecorator(0, CacheSetting.NoCache, new RedirectResourceResult(popupUriTemplate, new Dictionary<string, object> { { ResourceParameter.RequestId.Name, request.RequestId.ToString() } }));
        }
    }
}