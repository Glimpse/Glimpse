using System.Web.UI;
using System.Web.UI.WebControls;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet.Controls
{
    public class Client : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (GlimpseRuntime.IsAvailable)
            {
                writer.Write(GlimpseRuntime.Instance.GenerateScriptTags(GlimpseRuntime.Instance.CurrentRequestContext));
            }
        }
    }
}
