using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Extensibility;
using Glimpse.WebApi.Extensibility;
using Glimpse.WebApi.Model;
using System.Web.Http;
using System.Reflection;
using System.Web.Http.Filters;
using System.Collections.Concurrent;

namespace Glimpse.WebApi.Tab
{
//    class Configuration : WebApiTab, ILayoutControl
//    {
//        public override object GetData(ITabContext context)
//        {
//            var WebApiConfig = GlobalConfiguration.Configuration;

//            var properties = new List<KeyValuePair<object, object>>();

//            properties.Add(new KeyValuePair<object, object>("Include Error Detail Policy", WebApiConfig.IncludeErrorDetailPolicy.ToString()));
//            //properties.Add(new KeyValuePair<object, object>("Virtual Path", WebApiConfig.VirtualPathRoot));
//            //properties.AddRange(WebApiConfig.Properties);

//            var formatters = new List<FormatterModel>();

//            formatters.Add(new FormatterModel{
//                Name = "Form Url Encoded Formatter",
//                //MaxDepth = WebApiConfig.Formatters.FormUrlEncodedFormatter.MaxDepth,
//                //ReadBufferSize = WebApiConfig.Formatters.FormUrlEncodedFormatter.ReadBufferSize,
//                //SupportedEncodings = WebApiConfig.Formatters.FormUrlEncodedFormatter.SupportedEncodings,
//                SupportedMediaTypes = WebApiConfig.Formatters.FormUrlEncodedFormatter.SupportedMediaTypes.Select(m => m.MediaType).ToList(),
//                MediaTypeMappings = WebApiConfig.Formatters.FormUrlEncodedFormatter.MediaTypeMappings
//            });

//            formatters.Add(new FormatterModel
//            {
//                Name = "Json Formatter",
//                //Indent = WebApiConfig.Formatters.JsonFormatter.Indent,
//                //MaxDepth = WebApiConfig.Formatters.JsonFormatter.MaxDepth,
//                //SupportedEncodings = WebApiConfig.Formatters.JsonFormatter.SupportedEncodings,
//                SupportedMediaTypes = WebApiConfig.Formatters.JsonFormatter.SupportedMediaTypes.Select(m => m.MediaType).ToList(),
//                MediaTypeMappings = WebApiConfig.Formatters.JsonFormatter.MediaTypeMappings
//            });

//            formatters.Add(new FormatterModel
//            {
//                Name = "Xml Formatter",
//                //Indent = WebApiConfig.Formatters.XmlFormatter.Indent,
//                //MaxDepth = WebApiConfig.Formatters.XmlFormatter.MaxDepth,
//                //SupportedEncodings = WebApiConfig.Formatters.XmlFormatter.SupportedEncodings,
//                SupportedMediaTypes = WebApiConfig.Formatters.XmlFormatter.SupportedMediaTypes.Select(m => m.MediaType).ToList(),
//                MediaTypeMappings = WebApiConfig.Formatters.XmlFormatter.MediaTypeMappings
//            });

//            return new ConfigurationModel
//            {
//                Formatters = formatters,
//                Properties = properties
////                ContraintResolver = WebApiConfig.Initializer.Method..
//            };
//        }

//        public override string Name
//        {
//            get { return "WebAPI Configuration"; }
//        }

//        public bool KeysHeadings
//        {
//            get { return true; }
//        }
//    }

    //public class DependancyResolver : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        return WebApiConfig.DependencyResolver;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI Dependancy Resolver"; }
    //    }
    //}

    //public class Initializer : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        return WebApiConfig.Initializer;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI Initializer"; }
    //    }
    //}

    //public class MessageHandlers : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        return WebApiConfig.MessageHandlers;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI MessageHandlers"; }
    //    }
    //}

    //public class ParameterBindingRules : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        return WebApiConfig.ParameterBindingRules;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI ParameterBindingRules"; }
    //    }
    //}

    //public class Properties : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        return WebApiConfig.Properties;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI Properties"; }
    //    }
    //}


    //public class Filters : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        //var result = new List<KeyValuePair<string, string>>();

    //        //// Filters
    //        //result.AddRange(WebApiConfig.Filters.Select(f => new KeyValuePair<string, string>("Filter", f.GetType().ToString())));
    //        //result.AddRange(WebApiConfig.Filters.Select(f => new KeyValuePair<string, string>("Filter Instance", f.Instance.GetType().ToString())));
    //        //result.AddRange(WebApiConfig.Filters.Select(f => new KeyValuePair<string, string>("Filter Instance Allow Multiple", f.Instance.AllowMultiple.ToString())));

    //        //return System.Web.Mvc.GlobalFilters.Filters;
    //        return WebApiConfig.Filters.Select(f => new FilterModel { Type = f.Instance.GetType().Name, AllowMultiple = f.Instance.AllowMultiple, Scope = f.Scope.ToString() });
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI Filters"; }
    //    }
    //}

    //public class Formatters : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        var result = new List<KeyValuePair<string, string>>();

    //        // Formatters
    //        result.Add(new KeyValuePair<string, string>("Form Url Encoded Formatter", WebApiConfig.Formatters.FormUrlEncodedFormatter.GetType().ToString()));
    //        result.Add(new KeyValuePair<string, string>("Json Formatter", WebApiConfig.Formatters.JsonFormatter.GetType().ToString()));
    //        result.Add(new KeyValuePair<string, string>("Xml Formatter", WebApiConfig.Formatters.XmlFormatter.GetType().ToString()));

    //        return WebApiConfig.Formatters;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI Formatters"; }
    //    }
    //}

    //public class Services : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.Configuration;

    //        var allSingleServices = WebApiConfig.Services.GetType()
    //                                .GetField("_cacheSingle", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(WebApiConfig.Services);

    //        var listofServices = (allSingleServices as ConcurrentDictionary<Type, object>)
    //                                .Where(x => x.Value != null)
    //                                .OrderBy(x => x.Key.FullName)
    //                                .Select(x => new ServiceModel { Name = x.Value.GetType().FullName, Type = x.Key.FullName });

    //        return listofServices;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI Services"; }
    //    }
    //}

    //public class DefaultHandler : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.DefaultHandler;

    //        return WebApiConfig;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI DefaultHandler"; }
    //    }
    //}
    //public class DefaultServer : WebApiTab
    //{
    //    public override object GetData(ITabContext context)
    //    {
    //        var WebApiConfig = GlobalConfiguration.DefaultServer;

    //        return WebApiConfig;
    //    }

    //    public override string Name
    //    {
    //        get { return "WebAPI DefaultServer"; }
    //    }
    //}
}
