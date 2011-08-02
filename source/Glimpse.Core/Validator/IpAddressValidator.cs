using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
    public class IpAddressValidator : IGlimpseValidator, INotifiyGlimpseConfigChanged
    {
        GlimpseConfiguration Configuration { get; set; }
        ICollection<IIpFilter> IpFilters { get; set; }
        
        [ImportingConstructor]
        public IpAddressValidator(GlimpseConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            Configuration = configuration;
            
            IpFilters = BuildFilters(Configuration).ToList();
        }

        public bool IsValid(HttpContextBase context, LifecycleEvent lifecycleEvent)
        {
            if (IpFilters.Count == 0) return true; //no configured list, allow all IP's

            var userIpAddress = GetUserIpAddress(context);

            return IpFilters.Any(f => f.IsValid(userIpAddress));
        }

        public void ConfigChanged()
        {
            IpFilters = BuildFilters(Configuration).ToList();
        }

        public static IEnumerable<IIpFilter> BuildFilters(GlimpseConfiguration configuration)
        {
            var filters = configuration.IpAddresses.Cast<IpAddress>()
                .OrderBy(i => i.Address == null ? 1 : 0).ToList(); //Order so address are validated against before ranges

            foreach (var filter in filters)
            {
                if ((filter.Address == null && filter.AddressRange == null)
                    || (filter.Address != null && filter.AddressRange != null))
                    throw new ConfigurationErrorsException("IpAddress element must have either an address or an address-range attribute");

                if (filter.Address != null)
                    yield return new IpFilter(IPAddress.Parse(filter.Address));
                else
                    yield return new IpRangeFilter(filter.AddressRange);
            }
        }

        public IPAddress GetUserIpAddress(HttpContextBase context)
        {
            if (Configuration.IpForwardingEnabled)
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

        public interface IIpFilter
        {
            bool IsValid(IPAddress ip);
        }

        public class IpFilter : IIpFilter
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

        public class IpRangeFilter : IIpFilter
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