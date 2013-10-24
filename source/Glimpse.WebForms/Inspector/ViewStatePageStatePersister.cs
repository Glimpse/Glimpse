using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;

namespace Glimpse.WebForms.Inspector
{
    public class ViewStatePageStatePersister : HiddenFieldPageStatePersister
    {
        public ViewStatePageStatePersister(Page page, PageStatePersister innerPageStatePersister)
            : base(page)
        {
            Logger = GlimpseConfiguration.GetLogger();
            InnerPageStatePersister = innerPageStatePersister;
        }

        private ILogger Logger { get; set; }

        private PageStatePersister InnerPageStatePersister { get; set; }

        public override void Save()
        {
            Logger.Debug("PageStatePersister.Save() being executed - {0}", HttpContext.Current.Request.RawUrl);

            var controlTree = new Dictionary<string, Type>();
            GetControlTree(controlTree, Page.Controls);

            Logger.Debug("ControlTree contains '{0}' controls.", controlTree.Count);

            HttpContext.Current.Items.Add("_GlimpseWebFormViewState", ViewState);
            HttpContext.Current.Items.Add("_GlimpseWebFormControlTreeType", controlTree);

            if (InnerPageStatePersister != null)
            {
                Logger.Debug("Inner PageStatePersister.Save() being executed - {0}", InnerPageStatePersister.GetType());

                InnerPageStatePersister.Save();
            }
            else
            {
                // Only want to run ours if we don't have an inner
                base.Save();
            }
        }

        public override void Load()
        {
            if (InnerPageStatePersister != null)
            {
                Logger.Debug("Inner PageStatePersister.Load() being executed - {0}", InnerPageStatePersister.GetType());

                InnerPageStatePersister.Load();
            }
            else
            {
                // Only want to run ours if we don't have an inner
                base.Load();
            } 
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