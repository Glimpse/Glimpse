using System.Web.UI;
using Glimpse.WebForms.Tab;

namespace Glimpse.WebForms.Inspector
{
    public class ViewStatePageAdapter : System.Web.UI.Adapters.PageAdapter
    {
        public override PageStatePersister GetStatePersister()
        {
            return new ViewStatePageStatePersister(Page);
        } 
    }
}