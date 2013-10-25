using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
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
        private static readonly FieldInfo browserField = typeof(System.Web.UI.Adapters.ControlAdapter).GetField("_browser", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo controlField = typeof(System.Web.UI.Adapters.ControlAdapter).GetField("_control", BindingFlags.Instance | BindingFlags.NonPublic);

        public ViewStatePageAdapter()
        {
            InnerAdapter = Activator.CreateInstance<TPageAdapter>();
        }

        public override PageStatePersister GetStatePersister()
        {
            CopyAccessState();
            return new ViewStatePageStatePersister(Page, InnerAdapter.GetStatePersister());
        }

        public override StringCollection CacheVaryByHeaders
        {
            get
            {
                CopyAccessState();
                return InnerAdapter.CacheVaryByHeaders;
            }
        }

        public override StringCollection CacheVaryByParams
        {
            get
            {
                CopyAccessState();
                return InnerAdapter.CacheVaryByParams;
            }
        }

        private System.Web.UI.Adapters.PageAdapter InnerAdapter { get; set; }

        private bool PageAdapterStateCopied { get; set; }

        public override NameValueCollection DeterminePostBackMode()
        {
            CopyAccessState();
            return InnerAdapter.DeterminePostBackMode();
        }

#if NET45Plus
        public override NameValueCollection DeterminePostBackModeUnvalidated()
        {
            CopyAccessState();
            return InnerAdapter.DeterminePostBackModeUnvalidated();
        }
#endif

        public override ICollection GetRadioButtonsByGroup(string groupName)
        {
            CopyAccessState();
            return InnerAdapter.GetRadioButtonsByGroup(groupName);
        }

        public override void RegisterRadioButton(RadioButton radioButton)
        {
            CopyAccessState();
            InnerAdapter.RegisterRadioButton(radioButton);
        }

        public override void RenderBeginHyperlink(HtmlTextWriter writer, string targetUrl, bool encodeUrl, string softkeyLabel)
        {
            CopyAccessState();
            InnerAdapter.RenderBeginHyperlink(writer, targetUrl, encodeUrl, softkeyLabel);
        }

        public override void RenderBeginHyperlink(HtmlTextWriter writer, string targetUrl, bool encodeUrl, string softkeyLabel, string accessKey)
        {
            CopyAccessState();
            InnerAdapter.RenderBeginHyperlink(writer, targetUrl, encodeUrl, softkeyLabel, accessKey);
        }

        public override void RenderEndHyperlink(HtmlTextWriter writer)
        {
            CopyAccessState();
            InnerAdapter.RenderEndHyperlink(writer);
        }

        public override void RenderPostBackEvent(HtmlTextWriter writer, string target, string argument, string softkeyLabel, string text)
        {
            CopyAccessState();
            InnerAdapter.RenderPostBackEvent(writer, target, argument, softkeyLabel, text);
        }

        public override void RenderPostBackEvent(HtmlTextWriter writer, string target, string argument, string softkeyLabel, string text, string postUrl, string accessKey)
        {
            CopyAccessState();
            InnerAdapter.RenderPostBackEvent(writer, target, argument, softkeyLabel, text, postUrl, accessKey);
        }

        public override string TransformText(string text)
        {
            CopyAccessState();
            return InnerAdapter.TransformText(text);
        }

        private static readonly MethodInfo BeginRenderInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("BeginRender", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void BeginRender(HtmlTextWriter writer)
        {
            CopyAccessState();
            BeginRenderInfo.Invoke(InnerAdapter, new object[] { writer });
        }

        private static readonly MethodInfo CreateChildControlsInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("CreateChildControls", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void CreateChildControls()
        {
            CopyAccessState();
            CreateChildControlsInfo.Invoke(InnerAdapter, null);
        }

        private static readonly MethodInfo EndRenderInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("EndRender", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void EndRender(HtmlTextWriter writer)
        {
            CopyAccessState();
            EndRenderInfo.Invoke(InnerAdapter, new object[] { writer });
        }

        private static readonly MethodInfo GetPostBackFormReferenceInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("GetPostBackFormReference", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override string GetPostBackFormReference(string formId)
        {
            CopyAccessState();
            return (string)GetPostBackFormReferenceInfo.Invoke(InnerAdapter, new object[] { formId });
        }

        private static readonly MethodInfo LoadAdapterControlStateInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("LoadAdapterControlState", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void LoadAdapterControlState(object state)
        {
            CopyAccessState();
            LoadAdapterControlStateInfo.Invoke(InnerAdapter, new object[] { state });
        }

        private static readonly MethodInfo LoadAdapterViewStateInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("LoadAdapterViewState", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void LoadAdapterViewState(object state)
        {
            LoadAdapterViewStateInfo.Invoke(InnerAdapter, new object[] { state });
        }

        private static readonly MethodInfo OnInitInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("OnInit", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void OnInit(EventArgs e)
        {
            CopyAccessState();
            OnInitInfo.Invoke(InnerAdapter, new object[] { e });
        }

        private static readonly MethodInfo OnLoadInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("OnLoad", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void OnLoad(EventArgs e)
        {
            CopyAccessState();
            OnLoadInfo.Invoke(InnerAdapter, new object[] { e });
        }

        private static readonly MethodInfo OnPreRenderInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("OnPreRender", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void OnPreRender(EventArgs e)
        {
            CopyAccessState();
            OnPreRenderInfo.Invoke(InnerAdapter, new object[] { e });
        }

        private static readonly MethodInfo OnUnloadInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("OnUnload", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void OnUnload(EventArgs e)
        {
            CopyAccessState();
            OnUnloadInfo.Invoke(InnerAdapter, new object[] { e });
        }

        private static readonly MethodInfo RenderInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("Render", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void Render(HtmlTextWriter writer)
        {
            CopyAccessState();
            RenderInfo.Invoke(InnerAdapter, new object[] { writer });
        }

        private static readonly MethodInfo RenderChildrenInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("RenderChildren", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override void RenderChildren(HtmlTextWriter writer)
        {
            CopyAccessState();
            RenderChildrenInfo.Invoke(InnerAdapter, new object[] { writer });
        }

        private static readonly MethodInfo SaveAdapterControlStateInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("SaveAdapterControlState", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override object SaveAdapterControlState()
        {
            CopyAccessState();
            return SaveAdapterControlStateInfo.Invoke(InnerAdapter, null);
        }

        private static readonly MethodInfo SaveAdapterViewStateInfo = typeof(System.Web.UI.Adapters.PageAdapter).GetMethod("SaveAdapterViewState", BindingFlags.Instance | BindingFlags.NonPublic);
        protected override object SaveAdapterViewState()
        {
            CopyAccessState();
            return SaveAdapterViewStateInfo.Invoke(InnerAdapter, null);
        }

        private void CopyAccessState()
        {
            if (!PageAdapterStateCopied)
            { 
                browserField.SetValue(InnerAdapter, Browser);
                controlField.SetValue(InnerAdapter, Control);

                PageAdapterStateCopied = true;
            }
        }
    }
}