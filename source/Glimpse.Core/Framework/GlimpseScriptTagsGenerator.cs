using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Resource;
using Glimpse.Core.ResourceResult;
using Tavis.UriTemplates;

namespace Glimpse.Core.Framework
{
    /// <summary>
    /// Generator of Glimpse script tags
    /// </summary>
    public static class GlimpseScriptTagsGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="glimpseRequestId">The Glimpse request Id for the request for which script tags must be generated</param>
        /// <param name="configuration">A <see cref="IConfiguration"/></param>
        /// <param name="glimpseRuntimeVersion">The version of the glimpse runtime</param>
        /// <returns>The generated script tags</returns>
        public static string Generate(Guid glimpseRequestId, IReadonlyConfiguration configuration, string glimpseRuntimeVersion)
        {
            var encoder = configuration.HtmlEncoder;
            var clientScripts = configuration.ClientScripts;

            var stringBuilder = new StringBuilder();

            var scripts = clientScripts.OrderBy(cs => cs.Order).GroupBy(x => x.Order).ToDictionary(x => x.Key, y => y.ToList());
            foreach (var scriptGroup in scripts)
            {
                stringBuilder.AppendFormat(@"<script type='text/javascript' src='/glimpse.axd?n=glimpse_clientscripts&order={0}&{1}={2}'></script>", scriptGroup.Key, ClientScriptResource.GlimpseRequestId, glimpseRequestId);
            }


            return stringBuilder.ToString();
        }


    }
}