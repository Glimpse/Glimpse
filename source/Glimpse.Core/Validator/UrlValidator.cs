using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Text.RegularExpressions;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class UrlValidator:IGlimpseValidator{
        
        GlimpseConfiguration Configuration { get; set; }
        
        [ImportingConstructor]
        public UrlValidator(GlimpseConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            Configuration = configuration;
        }

        public bool IsValid(HttpContextBase context, LifecycleEvent lifecycleEvent)
        {
            if (Configuration.UrlBlackList.Count == 0) return true; //no configured list, allow all URL's

            foreach (GlimpseUrl url in Configuration.UrlBlackList)
            {
                if (Regex.IsMatch(context.Request.CurrentExecutionFilePath,url.Url,RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }

            // if nothing matched, the URL is allowed
            return true;
        }
    }
}