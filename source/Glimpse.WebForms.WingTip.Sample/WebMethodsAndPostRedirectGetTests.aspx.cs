using System;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using WingtipToys.Models;

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
            DoSomeProductQueryingAndReturnNumberOfProdutsFound();
            throw new Exception("This is a deliberately thrown exception");
        }

        void buttonServerTransferToDefaultPage_Click(object sender, EventArgs e)
        {
            DoSomeProductQueryingAndReturnNumberOfProdutsFound();
            Server.Transfer("/About.aspx");
        }

        void buttonRedirectToDefaultPage_Click(object sender, EventArgs e)
        {
            DoSomeProductQueryingAndReturnNumberOfProdutsFound();
            Response.Redirect("/default.aspx");
        }

        [WebMethod]
        public static string GetTheTime()
        {
            DoSomeProductQueryingAndReturnNumberOfProdutsFound();
            return DateTime.Now.ToString();
        }

        private static int DoSomeProductQueryingAndReturnNumberOfProdutsFound()
        {
            return new WingtipToys.Models.ProductContext().Products.ToList().Count;
        }
    }
}