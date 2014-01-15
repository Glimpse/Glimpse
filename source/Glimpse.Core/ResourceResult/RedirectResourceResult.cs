using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Tavis.UriTemplates;

namespace Glimpse.Core.ResourceResult
{
    /// <summary>
    /// The <see cref="IResourceResult"/> implementation responsible redirecting a client to a new Uri.
    /// </summary>
    public class RedirectResourceResult : IResourceResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectResourceResult" /> class without any parameter values in the Uri template.
        /// </summary>
        /// <param name="uriTemplate">The URI template.</param>
        public RedirectResourceResult(string uriTemplate) : this(uriTemplate, new Dictionary<string, object>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectResourceResult" /> class.
        /// </summary>
        /// <param name="uriTemplate">The Uri template. Uri templates should conform to <see href="http://tools.ietf.org/html/rfc6570"/>RFC 6570</param>.
        /// <param name="data">The Uri template parameter data.</param>
        /// <exception cref="System.ArgumentNullException">Exception thrown if <paramref name="uriTemplate"/> is <c>null</c>.</exception>
        /// <seealso href="http://tools.ietf.org/html/rfc6570">URI Template</seealso>
        public RedirectResourceResult(string uriTemplate, IDictionary<string, object> data)
        {
            if (string.IsNullOrEmpty(uriTemplate))
            {
                throw new ArgumentNullException("uriTemplate");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            UriTemplate = new UriTemplate(uriTemplate);
            Data = data;
        }

        private IDictionary<string, object> Data { get; set; }

        private UriTemplate UriTemplate { get; set; }

        /// <summary>
        /// Executes the resource result with the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Execute(IResourceResultContext context)
        {
            var data = Data;
            var uriTemplate = UriTemplate;
            foreach (var parameter in uriTemplate.GetParameterNames())
            {
                if (data.ContainsKey(parameter))
                {
                    uriTemplate.SetParameter(parameter, data[parameter]);
                }
            }

            var frameworkProvider = context.RequestResponseAdapter;
            frameworkProvider.SetHttpResponseStatusCode(301);
            frameworkProvider.SetHttpResponseHeader("Location", uriTemplate.Resolve());
        }
    }
}