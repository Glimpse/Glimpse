using System;
using System.Web;

namespace Glimpse.WebForms.Support
{
    public class AdapterManager
    {
        public static void Register(Type key, Type adapter)
        {
            var adapters = HttpContext.Current.Request.Browser.Adapters;
            if (!adapters.Contains(key.AssemblyQualifiedName))
            {
                adapters.Add(key.AssemblyQualifiedName, adapter.AssemblyQualifiedName);
            }
        }
    }
}
