using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Tavis.UriTemplates;

namespace Glimpse.Core.ResourceResult
{
    public class RedirectResourceResult : IResourceResult
    {
        public RedirectResourceResult(string uriTemplate) : this(uriTemplate, new Dictionary<string, object>())
        {
        }

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

            var frameworkProvider = context.FrameworkProvider;
            frameworkProvider.SetHttpResponseStatusCode(301);
            frameworkProvider.SetHttpResponseHeader("Location", uriTemplate.Resolve());
        }
    }
}