using System.Net;

namespace Glimpse.Net.Configuration
{
    public static class IpAddressValidity
    {
        public static bool IsValidIpAddress(this string address)
        {
            IPAddress outAddress;
            return IPAddress.TryParse(address, out outAddress);
        }
    }
}