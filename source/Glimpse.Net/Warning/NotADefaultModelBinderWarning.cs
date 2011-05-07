using System.Web.Mvc;

namespace Glimpse.Net.Warning
{
    internal class NotADefaultModelBinderWarning : Warning
    {
        public NotADefaultModelBinderWarning(IModelBinder modelBinder)
        {
            Message = modelBinder.GetType() + " is not a System.Web.Mvc.DefaultModelBinder.";
        }
    }
}
