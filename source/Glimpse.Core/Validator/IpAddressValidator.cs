using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using LukeSkywalker.IPNetwork;

namespace Glimpse.Core.Validator
{
    [GlimpseValidator]
    public class IpAddressValidator:IGlimpseValidator{
        
        public bool IsValid(HttpContextBase context, GlimpseConfiguration configuration, LifecycleEvent lifecycleEvent)
        {
            var ipFilters = BuildFilters(configuration).ToList();

            if (ipFilters.Count == 0) return true; //no configured list, allow all IP's

            var userIpAddress = GetUserIpAddress(context, configuration.IpForwardingEnabled);

            return ipFilters.Any(f => f.IsValid(userIpAddress));
        }

        static IEnumerable<IIpFilter> BuildFilters(GlimpseConfiguration configuration)
        {
            foreach (var ipAddress in configuration.IpAddresses.Cast<IpAddress>()
                .OrderBy(i => i.Address).ThenBy(i => i.AddressRange)) //Order so address are validated against before ranges
            {
                if((ipAddress.Address == null && ipAddress.AddressRange == null)
                    || (ipAddress.Address != null && ipAddress.AddressRange != null))
                    throw new ConfigurationErrorsException("IpAddress element must have either an address or an address-range attribute");

                if (ipAddress.Address != null)
                    yield return new IpFilter(IPAddress.Parse(ipAddress.Address));
                else
                    yield return new IpRangeFilter(ipAddress.AddressRange);
            }
        }

        static IPAddress GetUserIpAddress(HttpContextBase context, bool allowIpForwarding)
        {
            if (allowIpForwarding)
            {
                // Source http://support.appharbor.com/discussions/problems/681-requestuserhostaddress
                var forwardedFor = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(forwardedFor))
                {
                    // sometimes HTTP_X_FORWARDED_FOR returns multiple IP's
                    return IPAddress.Parse(forwardedFor.Split(',').Last());
                }
            }

            return IPAddress.Parse(context.Request.UserHostAddress);
        }

        private interface IIpFilter
        {
            bool IsValid(IPAddress ip);
        }

        class IpFilter : IIpFilter
        {
            IPAddress Ip { get; set; }

            public IpFilter(IPAddress ip)
            {
                Ip = ip;
            }

            public bool IsValid(IPAddress ip)
            {
                return Ip.Equals(ip);
            }
        }

        class IpRangeFilter : IIpFilter
        {
            IPNetwork Range { get; set; }

            public IpRangeFilter(string addressRange)
            {
                Range = IPNetwork.Parse(addressRange);
            }

            public bool IsValid(IPAddress ip)
            {
                return IPNetwork.Contains(Range, ip);
            }
        }
    }
}