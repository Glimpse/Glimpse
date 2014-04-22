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
            var time = DateTime.Now;
            var timeZoneInfo = TimeZoneInfo.Local;
            var isDaylightSavingTime = timeZoneInfo.IsDaylightSavingTime(time);

            return new
                {
                    serverName = Environment.MachineName,
                    user = Thread.CurrentPrincipal.Identity.Name,
                    serverTime = time,
                    serverTimezoneOffset = time.ToString("zz00"), // timeZoneInfo.BaseUtcOffset.Hours + (isDaylightSavingTime ? 1 : 0),
                    serverDaylightSavingTime = isDaylightSavingTime
                };
        }
    }
}