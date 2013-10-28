using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WingtipToys
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in Load");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in Init");
        }
        protected void Page_Render(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in Render");
        }
        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in SaveStateComplete");
        }
        protected void Page_SaveState(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in SaveState");
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in PreRender");
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in PreRenderComplete");
        }

        protected override void OnInitComplete(EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in InitComplete");
            base.OnInitComplete(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in PreInit");
            base.OnPreInit(e);
        }

        protected override void OnPreLoad(EventArgs e)
        {
            HttpContext.Current.Trace.Write("Something that happened in PreLoad");
            base.OnPreLoad(e);
        }

        private void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server.
            Exception exc = Server.GetLastError();

            // Handle specific exception.
            if (exc is InvalidOperationException)
            {
                // Pass the error on to the error page.
                Server.Transfer("ErrorPage.aspx?handler=Page_Error%20-%20Default.aspx",
                    true);
            }
        }
    }
}