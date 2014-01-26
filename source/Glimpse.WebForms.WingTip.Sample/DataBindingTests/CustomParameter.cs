using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WingtipToys.DataBindingTests
{
    [Obsolete]
    public class CustomParameter : Parameter
    {
        protected override object Evaluate(HttpContext context, Control control)
        {
            return "custom value";
        }
    }
}