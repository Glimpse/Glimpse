using Glimpse.Core.Extensibility;
using Glimpse.WebApi.Core.Extensibility;
using Glimpse.WebApi.Core.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace Glimpse.WebApi.Tab
{
    public class Configuration : WebApiTab, ILayoutControl
    {
        public override object GetData(ITabContext context)
        {
            var requestMessage = context.GetRequestContext<HttpContextBase>().Items["MS_HttpRequestMessage"] as HttpRequestMessage;
            var WebApiConfig = (requestMessage != null) ? requestMessage.GetConfiguration() : GlobalConfiguration.Configuration;

            var filters = WebApiConfig.Filters.Select(f => new FilterModel { Type = f.Instance.GetType().ToString(), AllowMultiple = f.Instance.AllowMultiple, Scope = f.Scope.ToString() });

            var formatters = new List<FormatterModel>();

            formatters.Add(new FormatterModel{
                Name = "Form Url Encoded Formatter",
                SupportedMediaTypes = WebApiConfig.Formatters.FormUrlEncodedFormatter.SupportedMediaTypes.Select(m => m.MediaType).ToList(),
                MediaTypeMappings = WebApiConfig.Formatters.FormUrlEncodedFormatter.MediaTypeMappings
            });

            formatters.Add(new FormatterModel
            {
                Name = "Json Formatter",
                SupportedMediaTypes = WebApiConfig.Formatters.JsonFormatter.SupportedMediaTypes.Select(m => m.MediaType).ToList(),
                MediaTypeMappings = WebApiConfig.Formatters.JsonFormatter.MediaTypeMappings
            });

            formatters.Add(new FormatterModel
            {
                Name = "Xml Formatter",
                SupportedMediaTypes = WebApiConfig.Formatters.XmlFormatter.SupportedMediaTypes.Select(m => m.MediaType).ToList(),
                MediaTypeMappings = WebApiConfig.Formatters.XmlFormatter.MediaTypeMappings
            });

            var properties = new Dictionary<string, object>();
            properties.Add("Include Error Detail Policy", WebApiConfig.IncludeErrorDetailPolicy.ToString());
            properties.Add("Client Certificate", requestMessage.GetClientCertificate());

            var allSingleServices = WebApiConfig.Services.GetType()
            .GetField("_cacheSingle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(WebApiConfig.Services);

            var services = (allSingleServices as ConcurrentDictionary<Type, object>)
                                    .Where(x => x.Value != null)
                                    .OrderBy(x => x.Key.FullName)
                                    .Select(x => {
                                        var serviceType = x.Value.GetType();
                                        var serviceName = serviceType.Namespace == "Castle.Proxies" 
                                            ? (x.Value as Castle.DynamicProxy.IProxyTargetAccessor).DynProxyGetTarget().GetType().FullName 
                                            : serviceType.FullName;
                                        return new ServiceModel { Name = serviceName, Type = x.Key.FullName };
                                    });

            return new ConfigurationModel
            {
                Filters = filters,
                Formatters = formatters,
                Properties = properties,
                Services = services
            };
        }

        public override string Name
        {
            get { return "WebAPI Configuration"; }
        }

        public bool KeysHeadings
        {
            get { return true; }
        }
    }
}
