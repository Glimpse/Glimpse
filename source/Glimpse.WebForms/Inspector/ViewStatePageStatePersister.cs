using System;
using System.Collections.Generic;
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
            var controlTree = new Dictionary<string, Type>();
            GetControlTree(controlTree, Page.Controls);

            HttpContext.Current.Items.Add("_GlimpseWebFormViewState", ViewState);
            HttpContext.Current.Items.Add("_GlimpseWebFormControlTreeType", controlTree);

            base.Save();
        }

        private void GetControlTree(Dictionary<string, Type> results, ControlCollection controls)
        { 
            foreach (Control control in controls)
            { 
                results.Add(control.UniqueID, control.GetType()); 
                if (control.HasControls())
                {
                    GetControlTree(results, control.Controls);
                }   
            } 
        }
    }
}