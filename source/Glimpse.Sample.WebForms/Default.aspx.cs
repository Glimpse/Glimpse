using System;
using System.Web.UI;
using Glimpse.Core.Extensibility;

namespace Glimpse.Sample.WebForms
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["Test"] = "Test session from WebForms";

            GlimpseTrace.Info("This is a glimpse trace from WebForms");
            Trace.Write("This is a standard trace from WebForms");
            Trace.Warn("Warning", "This is a webforms warning");
        }
    }
}
