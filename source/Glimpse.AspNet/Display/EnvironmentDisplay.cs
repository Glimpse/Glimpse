using System;
using System.Threading;
using Glimpse.Core.Extensibility;

namespace Glimpse.AspNet.Display
{
    [Obsolete]
    public class EnvironmentDisplay : IDisplay, IKey
    {
        private const string InternalName = "environment";

        public string Name
        {
            get { return InternalName; }
        }

        public string Key
        {
            get { return InternalName; }
        }

        public object GetData(ITabContext context)
        {
            var time = DateTimeOffset.Now;
            var timeZoneInfo = TimeZoneInfo.Local;
            var isDaylightSavingTime = timeZoneInfo.IsDaylightSavingTime(time);

            return new
                {
                    serverName = Environment.MachineName,
                    user = Thread.CurrentPrincipal.Identity.Name,
                    serverTime = time.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff"),
                    serverTimezoneOffset = time.ToString("zzz"),
                    serverDaylightSavingTime = isDaylightSavingTime
                };
        }
    }
}