using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using Glimpse.Core2.Extensibility;
using Glimpse.Core2.Framework;

namespace Glimpse.AspNet
{
    public class HttpHandlerEndpointConfiguration:ResourceEndpointConfiguration
    {
        private string applicationPath;
        public string ApplicationPath
        {
            get { return applicationPath ?? HttpContext.Current.Request.ApplicationPath; }
            set { applicationPath = value; }
        }

        protected override string GenerateUriTemplate(string resourceName, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger)
        {
            var root = VirtualPathUtility.ToAbsolute("~/", ApplicationPath);

            var stringBuilder = new StringBuilder(string.Format(@"{1}Glimpse.axd?n={0}", resourceName, root));

            var requiredParams = parameters.Where(p => p.IsRequired);
            foreach (var parameter in requiredParams)
            {
                stringBuilder.Append(string.Format("&{0}={{{1}}}", parameter.Name, parameter.Name));
            }

            var optionalParams = parameters.Except(requiredParams).Select(p=>p.Name).ToArray();

            //Format based on Form-style query continuation from RFC6570: http://tools.ietf.org/html/rfc6570#section-3.2.9
            if(optionalParams.Any())
                stringBuilder.Append(string.Format("{{&{0}}}", string.Join(",", optionalParams)));

            return stringBuilder.ToString();
        }
    }
}
