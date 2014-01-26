using System;
using System.Collections;
using System.Web.ModelBinding;
using System.Web.UI;

namespace WingtipToys.DataBindingTests
{
    public partial class ListView : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var text = TextBox1.Text;
            TextBox1.Text = "different text";
            ListView1.DataBind();
            TextBox1.Text = "more different text";
            ListView1.DataBind();
            TextBox1.Text = text;
            ListView1.DataBind();
        }

        public IEnumerable GetItems([Control("TextBox1")]  string filter, [QueryString("sort")] string order, [Custom] string custom)
        {
            if (filter == "different text")
            {
                yield return new { Id = 0 };
            }
            yield return new { Id = 1 };
            yield return new { Id = 2 };
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ListView1.DataBind();
            ListView2.DataBind();
            ListView2.DataBind();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ListView1.DataBind();
        }
    }
}