using System;
using System.Web;
using Glimpse.Core.Framework;

namespace Glimpse.WebForms.Support
{
    public class AdapterManager
    {
        public static void Register(Type key, Type adapter)
        {
            GlimpseConfiguration.GetLogger().Debug("Registering Adapter '{0}' as '{1}'", adapter.FullName, key.FullName);

            var adapters = HttpContext.Current.Request.Browser.Adapters;
            if (!adapters.Contains(key.AssemblyQualifiedName))
            {
                adapters.Add(key.AssemblyQualifiedName, adapter.AssemblyQualifiedName);
            }
        }
    }
}
