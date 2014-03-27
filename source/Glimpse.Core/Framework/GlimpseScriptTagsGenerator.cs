using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
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
        /// <returns>The generated script tags</returns>
        public static string Generate(Guid glimpseRequestId, IConfiguration configuration)
        {
            var encoder = configuration.HtmlEncoder;
            var resourceEndpoint = configuration.ResourceEndpoint;
            var clientScripts = configuration.ClientScripts;
            var logger = configuration.Logger;
            var resources = configuration.Resources;

            var stringBuilder = new StringBuilder();

            foreach (var clientScript in clientScripts.OrderBy(cs => cs.Order))
            {
                var dynamicScript = clientScript as IDynamicClientScript;
                if (dynamicScript != null)
                {
                    try
                    {
                        var requestTokenValues = new Dictionary<string, string>
                                         {
                                             { ResourceParameter.RequestId.Name, glimpseRequestId.ToString() },
                                             { ResourceParameter.VersionNumber.Name, configuration.Version },
                                             { ResourceParameter.Hash.Name, configuration.Hash }
                                         };

                        var resourceName = dynamicScript.GetResourceName();
                        var resource = resources.FirstOrDefault(r => r.Name.Equals(resourceName, StringComparison.InvariantCultureIgnoreCase));

                        if (resource == null)
                        {
                            logger.Warn(Resources.RenderClientScriptMissingResourceWarning, clientScript.GetType(), resourceName);
                            continue;
                        }

                        var uriTemplate = resourceEndpoint.GenerateUriTemplate(resource, configuration.EndpointBaseUri, logger);

                        var resourceParameterProvider = dynamicScript as IParameterValueProvider;

                        if (resourceParameterProvider != null)
                        {
                            resourceParameterProvider.OverrideParameterValues(requestTokenValues);
                        }

                        var template = SetParameters(new UriTemplate(uriTemplate), requestTokenValues);
                        var uri = encoder.HtmlAttributeEncode(template.Resolve());

                        if (!string.IsNullOrEmpty(uri))
                        {
                            stringBuilder.AppendFormat(@"<script type='text/javascript' src='{0}'></script>", uri);
                        }

                        continue;
                    }
                    catch (Exception exception)
                    {
                        logger.Error(Core.Resources.GenerateScriptTagsDynamicException, exception, dynamicScript.GetType());
                    }
                }

                var staticScript = clientScript as IStaticClientScript;
                if (staticScript != null)
                {
                    try
                    {
                        var uri = encoder.HtmlAttributeEncode(staticScript.GetUri(configuration.Version));

                        if (!string.IsNullOrEmpty(uri))
                        {
                            stringBuilder.AppendFormat(@"<script type='text/javascript' src='{0}'></script>", uri);
                        }

                        continue;
                    }
                    catch (Exception exception)
                    {
                        logger.Error(Core.Resources.GenerateScriptTagsStaticException, exception, staticScript.GetType());
                    }
                }

                logger.Warn(Core.Resources.RenderClientScriptImproperImplementationWarning, clientScript.GetType());
            }

            return stringBuilder.ToString();
        }

        private static UriTemplate SetParameters(UriTemplate template, IEnumerable<KeyValuePair<string, string>> nameValues)
        {
            if (nameValues == null)
            {
                return template;
            }

            foreach (var pair in nameValues)
            {
                template.SetParameter(pair.Key, pair.Value);
            }

            return template;
        }
    }
}