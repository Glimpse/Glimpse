using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Glimpse.WebForms
{
    public class PreApplicationStartCode
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(Module));
        }
    }
}