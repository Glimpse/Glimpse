using System.Linq;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    internal class IpAddressValidator:IGlimpseValidator{
        public bool IsValid(HttpContextBase context, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent)
        {
            if (configuration.IpAddresses.Count == 0) return true; //no configured list, allow all IP's

            var userIpAddress = GetUserIpAddress(context, configuration.AllowIPForwarding);

            return configuration.IpAddresses.Contains(userIpAddress);
        }

        static string GetUserIpAddress(HttpContextBase context, bool allowIPForwarding)
        {
            if (allowIPForwarding)
            {
                // Source http://support.appharbor.com/discussions/problems/681-requestuserhostaddress
                var forwardedFor = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    // sometimes HTTP_X_FORWARDED_FOR returns multiple IP's
                    return forwardedFor.Split(',').Last();
                }
            }

            return context.Request.UserHostAddress;
        }
    }
}