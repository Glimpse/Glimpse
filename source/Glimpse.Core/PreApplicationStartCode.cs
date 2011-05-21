using Glimpse.WebForms;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Glimpse.Core
{
    public class PreApplicationStartCode
    {
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(Module));
        }
    }
}