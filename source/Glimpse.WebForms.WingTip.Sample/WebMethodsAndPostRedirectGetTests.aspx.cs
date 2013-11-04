using System;
using System.Web.Services;
using System.Web.UI;

namespace WingtipToys
{
    public partial class WebMethodsAndPostRedirectGetTests : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.buttonRedirectToDefaultPage.Click += buttonRedirectToDefaultPage_Click;
            this.buttonServerTransferToDefaultPage.Click += buttonServerTransferToDefaultPage_Click;
            this.buttonThrowAnException.Click += buttonThrowAnException_Click;
        }

        void buttonThrowAnException_Click(object sender, EventArgs e)
        {
            throw new Exception("This is a deliberately thrown exception");
        }

        void buttonServerTransferToDefaultPage_Click(object sender, EventArgs e)
        {
            Server.Transfer("/About.aspx");
        }

        void buttonRedirectToDefaultPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("/");
        }

        [WebMethod]
        public static string GetTheTime()
        {
            return DateTime.Now.ToString();
        }
    }
}