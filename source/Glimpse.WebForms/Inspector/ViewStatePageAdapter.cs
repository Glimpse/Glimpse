using System;
using System.Web.UI;
using Glimpse.WebForms.Tab;

namespace Glimpse.WebForms.Inspector
{
    public class ViewStatePageAdapter : System.Web.UI.Adapters.PageAdapter
    {
        public override PageStatePersister GetStatePersister()
        {
            return new ViewStatePageStatePersister(Page, null);
        } 
    }

    public class ViewStatePageAdapter<TPageAdapter> : System.Web.UI.Adapters.PageAdapter
        where TPageAdapter : System.Web.UI.Adapters.PageAdapter
    {
        public ViewStatePageAdapter()
        {
            InnerAdapter = Activator.CreateInstance<TPageAdapter>();
        }

        private System.Web.UI.Adapters.PageAdapter InnerAdapter { get; set; }

        public override PageStatePersister GetStatePersister()
        {
            return new ViewStatePageStatePersister(Page, InnerAdapter.GetStatePersister());
        }
    }
}