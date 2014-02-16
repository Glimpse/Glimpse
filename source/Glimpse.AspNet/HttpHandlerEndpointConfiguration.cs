using System;
using System.Collections.Generic;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet
{
    public class HttpHandlerEndpointConfiguration : UriTemplateResourceEndpointConfiguration
    {
        private string applicationPath;
        
        public string ApplicationPath
        {
            get { return applicationPath ?? HttpRuntime.AppDomainAppVirtualPath; } // HttpRuntime call based on http://mvolo.com/iis7-integrated-mode-request-is-not-available-in-this-context-exception-in-applicationstart/
            set { applicationPath = value; }
        }

        public override bool IsResourceRequest(Uri requestUri, string endpointBaseUri)
        {
            endpointBaseUri = VirtualPathUtility.ToAbsolute(endpointBaseUri, ApplicationPath);

            return base.IsResourceRequest(requestUri, endpointBaseUri);
        }

        protected override string GenerateUriTemplate(string resourceName, string baseUri, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger)
        {
            baseUri = VirtualPathUtility.ToAbsolute(baseUri, ApplicationPath);

            return base.GenerateUriTemplate(resourceName, baseUri, parameters, logger);
        }
    }
}