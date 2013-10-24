using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
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

        public override StringCollection CacheVaryByHeaders
        {
            get { return InnerAdapter.CacheVaryByHeaders; }
        }

        public override StringCollection CacheVaryByParams
        {
            get { return InnerAdapter.CacheVaryByParams; }
        }

        public override NameValueCollection DeterminePostBackMode()
        {
            return InnerAdapter.DeterminePostBackMode();
        }

        public override NameValueCollection DeterminePostBackModeUnvalidated()
        {
            return InnerAdapter.DeterminePostBackModeUnvalidated();
        }

        public override ICollection GetRadioButtonsByGroup(string groupName)
        {
            return InnerAdapter.GetRadioButtonsByGroup(groupName);
        }

        public override void RegisterRadioButton(RadioButton radioButton)
        {
            InnerAdapter.RegisterRadioButton(radioButton);
        }

        public override void RenderBeginHyperlink(HtmlTextWriter writer, string targetUrl, bool encodeUrl, string softkeyLabel)
        {
            InnerAdapter.RenderBeginHyperlink(writer, targetUrl, encodeUrl, softkeyLabel);
        }

        public override void RenderBeginHyperlink(HtmlTextWriter writer, string targetUrl, bool encodeUrl, string softkeyLabel, string accessKey)
        {
            InnerAdapter.RenderBeginHyperlink(writer, targetUrl, encodeUrl, softkeyLabel, accessKey);
        }

        public override void RenderEndHyperlink(HtmlTextWriter writer)
        {
            InnerAdapter.RenderEndHyperlink(writer);
        }

        public override void RenderPostBackEvent(HtmlTextWriter writer, string target, string argument, string softkeyLabel, string text)
        {
            InnerAdapter.RenderPostBackEvent(writer, target, argument, softkeyLabel, text);
        }

        public override void RenderPostBackEvent(HtmlTextWriter writer, string target, string argument, string softkeyLabel, string text, string postUrl, string accessKey)
        {
            InnerAdapter.RenderPostBackEvent(writer, target, argument, softkeyLabel, text, postUrl, accessKey);
        }

        public override string TransformText(string text)
        {
            return InnerAdapter.TransformText(text);
        }
    }
}