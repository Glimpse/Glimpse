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
            return new
                {
                    serverName = Environment.MachineName,
                    user = Thread.CurrentPrincipal.Identity.Name
                };
        }
    }
}