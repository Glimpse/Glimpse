using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Plumbing;

namespace Glimpse.Core.Handlers
{
    [GlimpseHandler]
    public class JavascriptData:IGlimpseHandler
    {
        public IGlimpseMetadataStore MetadataStore { get; set; }
        public GlimpseSerializer Serializer { get; set; }

        [ImportingConstructor]
        public JavascriptData(IGlimpseMetadataStore metadataStore, GlimpseSerializer serializer)
        {
            MetadataStore = metadataStore;
            Serializer = serializer;
        }

        public string ResourceName
        {
            get { return "data.js"; }
        }

        public void ProcessRequest(HttpContextBase context)
        {
            var response = context.Response;
            var id = context.Request.QueryString["id"];

            var data = from request in MetadataStore.Requests
                   where request.RequestId.ToString().Equals(id)
                   select request;

            var requestResult = data.FirstOrDefault();
            if (requestResult != null)
            {
                var path = context.GlimpseResourcePath("");
                var javascript = string.Format(@"var glimpse = {0}, glimpsePath = '{1}';", requestResult.Json, path);


                response.Write(javascript);
            
                response.AddHeader("Content-Type", "application/x-javascript");
            }
        }
    }
}
