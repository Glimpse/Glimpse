using System.Web.UI;
using System.Web.UI.WebControls;
using Glimpse.Core.Framework;

namespace Glimpse.AspNet.Controls
{
    public class Client : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (GlimpseRuntime.IsInitialized)
            {
                var aspNetRequestResponseAdapter = GlimpseRuntime.Instance.CurrentRequestContext.RequestResponseAdapter as IAspNetRequestResponseAdapter;
                if (aspNetRequestResponseAdapter != null)
                {
                    writer.Write(aspNetRequestResponseAdapter.GenerateGlimpseScriptTags());
                }
            }
        }
    }
}
