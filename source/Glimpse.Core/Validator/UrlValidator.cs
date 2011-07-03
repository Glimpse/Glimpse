using System.Web;
using System.Text.RegularExpressions;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class UrlValidator:IGlimpseValidator{
        public bool IsValid(HttpContextBase context, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent)
        {
            if (configuration.UrlBlackList.Count == 0) return true; //no configured list, allow all URL's

            foreach (GlimpseUrl url in configuration.UrlBlackList)
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