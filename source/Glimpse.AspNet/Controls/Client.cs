using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Glimpse.AspNet.Extensions;

namespace Glimpse.AspNet.Controls
{
    public class Client : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            var tags = new HttpContextWrapper(Context).GenerateGlimpseScriptTags();

            writer.Write(tags);
        }
    }
}
