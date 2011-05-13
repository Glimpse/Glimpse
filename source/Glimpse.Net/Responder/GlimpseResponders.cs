using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Glimpse.Net.Configuration;
using Glimpse.Net.Extensibility;
using Glimpse.Net.Extensions;
using Glimpse.Net.Sanitizer;
using Glimpse.Net.Warning;
using Newtonsoft.Json;

namespace Glimpse.Net.Responder
{
    public class GlimpseResponders
    {
        [ImportMany] private IList<IGlimpseConverter> JsConverters { get; set; }
        [Export] public JavaScriptSerializer JsSerializer { get; set; }
        [ImportMany] public IList<GlimpseResponder> Outputs { get; set; }
        public const string RootPath = "Glimpse/";
        private IGlimpseSanitizer Sanitizer { get; set; }

        public GlimpseResponders()
        {
            JsSerializer = new JavaScriptSerializer();
            JsConverters = new List<IGlimpseConverter>();
            Outputs = new List<GlimpseResponder>();
            Sanitizer = new CSharpSanitizer();
        }

        public GlimpseResponder GetResponderFor(HttpApplication application)
        {
            var path = application.Request.Path;
            var store = application.Context.Items;

            var result = (from o in Outputs where path.ToLower().Contains((RootPath + o.ResourceName).ToLower()) select o).SingleOrDefault();
            
            store[GlimpseConstants.ValidPath] = true;

            if (result == null) 
                store[GlimpseConstants.ValidPath] = false;

            return result;
        }

        public void RegisterConverters()
        {
            JsSerializer.RegisterConverters(JsConverters);
        }

        public string StandardResponse(HttpApplication application, Guid requestId)
        {
            IDictionary<string, object> data;
            if (!application.TryGetData(out data)) return "Error: No Glimpse Data Found";
            var warnings = application.Context.GetWarnings();

            var sb = new StringBuilder("{");
            foreach (var item in data)
            {
                try
                {
                    var jsonSerializerSettings = new JsonSerializerSettings{ ContractResolver = new GlimpseContractResolver() };
                    jsonSerializerSettings.Error += (obj, args) =>
                    {
                        warnings.Add(new SerializationWarning(args.ErrorContext.Error));
                        args.ErrorContext.Handled = true;
                    };

                    string dataString = JsonConvert.SerializeObject(item.Value, Formatting.None, jsonSerializerSettings);
                    sb.Append(string.Format("\"{0}\":{1},", item.Key, dataString));
                }
                catch(Exception ex)
                {
                    var message = JsonConvert.SerializeObject(ex.Message, Formatting.None);
                    message = message.Remove(message.Length-1).Remove(0, 1);
                    var callstack = JsonConvert.SerializeObject(ex.StackTrace, Formatting.None);
                    callstack = callstack.Remove(callstack.Length - 1).Remove(0, 1);
                    const string helpMessage = "Please implement an IGlimpseConverter for the type mentioned above, or one of its base types, to fix this problem. More info on a better experience for this coming soon, keep an eye on <a href='http://getGlimpse.com' target='main'>getGlimpse.com</a></span>";

                    sb.Append(string.Format("\"{0}\":\"<span style='color:red;font-weight:bold'>{1}</span><br/>{2}</br><span style='color:black;font-weight:bold'>{3}</span>\",", item.Key, message, callstack, helpMessage));
                }
            }

            //Add exceptions tab if needed
            if (warnings.Count > 0)
            {
                var warningTable = new List<object[]>{new[]{"Type", "Message"}};
                warningTable.AddRange(warnings.Select(warning => new[] {warning.GetType().Name, warning.Message}));

                var dataString = JsSerializer.Serialize(warningTable);
                sb.Append(string.Format("\"{0}\":{1},", "GlimpseWarnings", dataString));
            }

            if (sb.Length > 1) sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            //var json = JsSerializer.Serialize(data); //serialize data to Json
            var json = sb.ToString();
            json = Sanitizer.Sanitize(json);

            //if ajax request, render glimpse data to headers
            if (application.IsAjax())
            {
                application.Response.AddHeader(GlimpseConstants.HttpHeader, requestId.ToString());
            }
            else
            {
                if (application.GetGlimpseMode() == GlimpseMode.On)
                {
                    var path = VirtualPathUtility.ToAbsolute("~/", application.Context.Request.ApplicationPath);
                    var html = string.Format(@"<script type='text/javascript' id='glimpseData' data-glimpse-requestID='{1}'>var glimpse = {0}, glimpsePath = '{2}';</script>", json, requestId, path);
                    html += @"<script type='text/javascript' id='glimpseClient' src='" + path + RootPath + "glimpseClient.js'></script>";
                    application.Response.Write(html);
                }
            }

            return json;
        }
    }
}
