using System.Web.Mvc;

namespace Glimpse.Mvc3.Warning
{
    internal class NotADefaultModelBinderWarning : Net.Warning.Warning
    {
        public NotADefaultModelBinderWarning(IModelBinder modelBinder)
        {
            Message = modelBinder.GetType() + " is not a System.Web.Mvc.DefaultModelBinder.";
        }
    }
}
