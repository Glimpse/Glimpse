using System.Web;
using System.Web.UI;

namespace Glimpse.WebForms.Inspector
{
    public class ViewStatePageStatePersister : HiddenFieldPageStatePersister
    {
        public ViewStatePageStatePersister(Page page)
            : base(page)
        {
        }

        public override void Save()
        {
            HttpContext.Current.Items.Add("_GlimpseWebFormViewState", ViewState);

            base.Save();
        }
    }
}