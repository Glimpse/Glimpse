using System;
using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Framework;
using Glimpse.Core.ResourceResult;

namespace Glimpse.Core.Resource
{
    public class PopupResource : IPrivilegedResource
    {
        public string Name
        {
            get { return "glimpse_popup"; }
        }

        public virtual IEnumerable<ResourceParameterMetadata> Parameters
        {
            get { return new[] { ResourceParameter.RequestId, ResourceParameter.VersionNumber }; }
        }

        public IResourceResult Execute(IResourceContext context)
        {
            throw new NotSupportedException(string.Format(Resources.RrivilegedResourceExecuteNotSupported, GetType().Name));
        }

        public IResourceResult Execute(IResourceContext context, IGlimpseConfiguration configuration)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (configuration == null) throw new ArgumentNullException("configuration");

            Guid requestId;

#if NET35
            if (!Glimpse.Core.Backport.Net35Backport.TryParseGuid(context.Parameters[ResourceParameter.RequestId.Name], out requestId))
                return new StatusCodeResourceResult(404);
#else
            if (!Guid.TryParse(context.Parameters.GetValueOrDefault(ResourceParameter.RequestId.Name), out requestId))
                return new StatusCodeResourceResult(404);
#endif

            string version = context.Parameters.GetValueOrDefault(ResourceParameter.VersionNumber.Name);

            if (string.IsNullOrEmpty(version))
                return new StatusCodeResourceResult(404);

            string scriptTags = configuration.GenerateScriptTags(requestId, version);

            string html = string.Format("<!DOCTYPE html><html><head><meta charset='utf-8'><title>Glimpse Popup</title></head><body class='glimpse-popup'>{0}</body></html>", scriptTags);

            return new HtmlResourceResult(html);
        }
    }
}