﻿using System.Web.Mvc;

namespace Glimpse.Mvc3.Warning
{
    internal class NotAControllerActionInvokerWarning:WebForms.Warning.Warning
    {
        public NotAControllerActionInvokerWarning(IActionInvoker actionInvoker)
        {
            Message = actionInvoker.GetType() + " is not a System.Web.Mvc.ControllerActionInvoker.";
        }
    }
}
