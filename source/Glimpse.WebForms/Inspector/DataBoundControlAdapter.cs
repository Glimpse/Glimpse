using Glimpse.Core.Framework;
using Glimpse.WebForms.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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
                return (Dictionary<string, List<DataBindParameterModel>>)HttpContext.Current.Items["_GlimpseWebFormDataBinding"];
            }
        }

        private TimeSpan Offset
        {
            get
            {
                return (GlimpseRuntime.IsInitialized ? GlimpseRuntime.Instance.CurrentRequestContext.CurrentExecutionTimer : UnavailableGlimpseRequestContext.Instance.CurrentExecutionTimer).Point().Offset;
            }
        }

        private List<KeyValuePair<string, ParameterCollection>> Parameters
        {
            get
            {
                var parameters = new List<KeyValuePair<string, ParameterCollection>>();
                var objectDataSource = DataBoundControl.DataSourceObject as ObjectDataSource;
                if (objectDataSource != null)
                {
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("Select", objectDataSource.SelectParameters));
                }
                var sqlDataSource = DataBoundControl.DataSourceObject as SqlDataSource;
                if (sqlDataSource != null)
                {
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("Select", sqlDataSource.SelectParameters));
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("Filter", sqlDataSource.FilterParameters));
                }
                var linqDataSource = DataBoundControl.DataSourceObject as LinqDataSource;
                if (linqDataSource != null)
                {
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("Select", linqDataSource.SelectParameters));
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("Where", linqDataSource.WhereParameters));
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("OrderBy", linqDataSource.OrderByParameters));
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("GroupBy", linqDataSource.GroupByParameters));
                    parameters.Add(new KeyValuePair<string, ParameterCollection>("OrderGroupsBy", linqDataSource.OrderGroupsByParameters));
                }
                return parameters;
            }
        }

        protected override void OnInit(EventArgs e)
        {
#if NET45Plus
            DataBoundControl.CallingDataMethods += DataBoundControl_CallingDataMethods;
#endif
            DataBoundControl.DataBinding += DataBoundControl_DataBinding;
            base.OnInit(e);
        }

#if NET45Plus
        protected void DataBoundControl_CallingDataMethods(object sender, CallingDataMethodsEventArgs e)
        {
            HttpContext.Current.Items["_GlimpseWebFormModelBinding"] = new DataBindParameterModel(Offset);
        }
#endif

        protected void DataBoundControl_DataBinding(object sender, EventArgs e)
        {
            var parameterModel = new DataBindParameterModel(Offset);
            foreach (var parameters in Parameters)
            {
                var values = parameters.Value.GetValues(HttpContext.Current, DataBoundControl);
                foreach (Parameter parameter in parameters.Value)
                {
                    var field = GetField(parameter);
                    var defaultValue = GetDefaultValue(parameter);
                    parameterModel.DataBindParameters.Add(new DataBindParameter(field, parameter.GetType().Name.Replace("Parameter", null), values[parameter.Name], parameters.Key, defaultValue));
                }
            }
#if NET45Plus
            if (HttpContext.Current.Items.Contains("_GlimpseWebFormModelBinding"))
            {
                parameterModel = (DataBindParameterModel)HttpContext.Current.Items["_GlimpseWebFormModelBinding"];
                HttpContext.Current.Items.Remove("_GlimpseWebFormModelBinding");
            }
#endif
            if (!DataBindInfo.ContainsKey(DataBoundControl.UniqueID))
            {
                DataBindInfo[DataBoundControl.UniqueID] = new List<DataBindParameterModel>();
            }
            DataBindInfo[DataBoundControl.UniqueID].Add(parameterModel);
        }

        private string GetField(Parameter parameter)
        {
            var defaultPropertyAttribute = Attribute.GetCustomAttributes(parameter.GetType()).First(a => a is DefaultPropertyAttribute) as DefaultPropertyAttribute;
            string field = null;
            if (defaultPropertyAttribute != null && defaultPropertyAttribute.Name != "DefaultValue")
            {
                field = parameter.GetType().GetProperty(defaultPropertyAttribute.Name).GetValue(parameter, null) as string;
            }
            return field ?? parameter.Name;
        }

        private object GetDefaultValue(Parameter parameter)
        {
            object defaultValue = parameter.DefaultValue;
            if (!string.IsNullOrEmpty(parameter.DefaultValue))
            {
                if (parameter.Type != TypeCode.Empty && parameter.Type != TypeCode.Object && parameter.Type != TypeCode.DBNull)
                {
                    defaultValue = Convert.ChangeType(parameter.DefaultValue, parameter.Type, CultureInfo.CurrentCulture);
                }
            }
            return defaultValue;
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
#if NET45Plus
            DataBoundControl.CallingDataMethods += DataBoundControl_CallingDataMethods;
#endif
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
