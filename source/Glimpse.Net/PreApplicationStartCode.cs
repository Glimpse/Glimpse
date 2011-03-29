using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Glimpse.Net
{
    public class PreApplicationStartCode
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(Module));
        }
    }
}
