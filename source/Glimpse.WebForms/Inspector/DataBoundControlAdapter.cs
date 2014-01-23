using Glimpse.WebForms.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls;

namespace Glimpse.WebForms.Inspector
{

    public class DataBoundControlAdapter : ControlAdapter
    {
        protected DataBoundControl DataBoundControl
        {
            get
            {
                return (DataBoundControl)Control;
            }
        }

        private static Dictionary<string, List<DataBindParameterModel>> DataBindInfo
        {
            get
            {
                return (Dictionary<string, List<DataBindParameterModel>>)HttpContext.Current.Items["_GlimpseWebFormDataBindingInfo"];
            }
        }

        protected override void OnInit(EventArgs e)
        {
            DataBoundControl.DataBinding += DataBoundControl_DataBinding;
            base.OnInit(e);
        }

        protected void DataBoundControl_DataBinding(object sender, EventArgs e)
        {
            DataBindParameterModel parameterModel = null;
            var dataSource = DataBoundControl.DataSourceObject as ObjectDataSource;
            if (dataSource != null)
            {
                parameterModel = new DataBindParameterModel(Glimpse.Core.Framework.GlimpseConfiguration.GetConfiguredTimerStrategy()().Point().Offset);
                var values = dataSource.SelectParameters.GetValues(HttpContext.Current, dataSource);
                foreach (Parameter parameter in dataSource.SelectParameters)
                {
                    parameterModel.DataBindParameters.Add(new DataBindParameter(parameter.Name, parameter.GetType(), values[parameter.Name]));
                }
            }
            if (parameterModel != null && parameterModel.DataBindParameters.Count > 0)
            {
                if (!DataBindInfo.ContainsKey(DataBoundControl.UniqueID))
                {
                    DataBindInfo[DataBoundControl.UniqueID] = new List<DataBindParameterModel>();
                }
                DataBindInfo[DataBoundControl.UniqueID].Add(parameterModel);
            }
        }
    }


    public class DataBoundControlAdapter<TControlAdapter> : DataBoundControlAdapter
        where TControlAdapter : ControlAdapter
    {
        private static readonly FieldInfo browserField = typeof(ControlAdapter).GetField("_browser", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo controlField = typeof(ControlAdapter).GetField("_control", BindingFlags.Instance | BindingFlags.NonPublic);

        public DataBoundControlAdapter()
        {
            InnerAdapter = Activator.CreateInstance<TControlAdapter>();
        }

        private ControlAdapter InnerAdapter { get; set; }

        private bool AdapterStateCopied { get; set; }

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
            DataBoundControl.DataBinding += DataBoundControl_DataBinding;
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
            if (!AdapterStateCopied)
            {
                browserField.SetValue(InnerAdapter, Browser);
                controlField.SetValue(InnerAdapter, Control);

                AdapterStateCopied = true;
            }
        }
    }
}
