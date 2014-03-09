using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.WebForms.Inspector;
using Glimpse.WebForms.Model;
using Glimpse.WebForms.Support;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Glimpse.WebForms.Tab
{
    public class ControlTree : AspNetTab, ITabSetup, ITabLayout, IKey
    {
        private static readonly MethodInfo traceContextVerifyStartMethod = typeof(System.Web.TraceContext).GetMethod("VerifyStart", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo requestDataField = typeof(System.Web.TraceContext).GetField("_requestData", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly ViewStateFormatter viewStateFormatter = new ViewStateFormatter();
        private static readonly DataBindFormatter dataBindFormatter = new DataBindFormatter();

        private static readonly object Layout = TabLayout.Create()
            .Row(r =>
            {
                r.Cell("controlId").AsKey().WithTitle("Control ID");
                r.Cell("type").WithTitle("Type");
                r.Cell("renderSize").Class("mono").AlignRight().WidthInPixels(150).WithTitle("Render (w/ children)").Suffix(" Bytes");
                r.Cell("viewstateSize").Class("mono").AlignRight().WidthInPixels(125).WithTitle("ViewState").Suffix(" Bytes");
                r.Cell("controlstateSize").Class("mono").AlignRight().WidthInPixels(125).WithTitle("ControlState").Suffix(" Bytes");
            }).Row(r =>
            {
                r.Cell("viewstateTitle").WithTitle("Title").AsMinimalDisplay().AlignRight().AsKey().Class("glimpse-sub-heading").PaddingRightInPercent(2);
                r.Cell("viewstate").WithTitle("ViewState").SpanColumns(4).AsMinimalDisplay();
            }).Row(r =>
            {
                r.Cell("dataBindParametersTitle").WithTitle("Title").AsMinimalDisplay().AlignRight().AsKey().Class("glimpse-sub-heading").PaddingRightInPercent(2);
                r.Cell("dataBindParameters").WithTitle("DataBindParameters").SpanColumns(4).AsMinimalDisplay();
            }).Build();

        public override string Name
        {
            get { return "Control Tree"; }
        }

        public string Key
        {
            get { return "glimpse_webforms_controltree"; }
        }

        public override RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginSessionAccess | RuntimeEvent.EndRequest; }
        }

        public object GetLayout()
        {
            return Layout;
        }

        public void Setup(ITabSetupContext context)
        {
            context.PersistMessages<PageLifeCycleMessage>();
        }

        public override object GetData(ITabContext context)
        {
            var trace = HttpContext.Current.Trace;

            var hasRun = context.TabStore.Get("hasRun");
            if (hasRun == null)
            {
                context.Logger.Debug("ControlTree Tab Initial Run - {0}", HttpContext.Current.Request.RawUrl);

                context.TabStore.Set("hasRun", "true");

                //Remember the previous state, turn tracing on at the begining of the request,
                //set things up so that when request is finished, lets put things back to the 
                //way they where. Lastly, make sure sate of trace is setup

                context.Logger.Debug("Setting logger infrastructure - previouslyEnabled = {0}", trace.IsEnabled);

                var previouslyEnabled = trace.IsEnabled;
                if (!previouslyEnabled)
                {
                    trace.IsEnabled = true;
                }

                trace.TraceFinished += (sender, args) =>
                {
                    context.Logger.Debug("Resetting logger infrastructure - previouslyEnabled = {0}", previouslyEnabled);
                    if (!previouslyEnabled)
                    {
                        trace.IsEnabled = false;
                    }
                };

                traceContextVerifyStartMethod.Invoke(trace, null);

                RegisterAdapters(context);

                return null;
            }

            context.Logger.Debug("ControlTree Tab Final Run - {0}", HttpContext.Current.Request.RawUrl);

            if (requestDataField != null)
            {
                var requestData = requestDataField.GetValue(trace) as DataSet;
                if (requestData != null)
                {
                    context.Logger.Debug("Pulling out the `Trace_Control_Tree` from internal logging infrastructure");

                    var treeData = ProcessData(requestData.Tables["Trace_Control_Tree"], context.Logger, context.GetMessages<PageLifeCycleMessage>());
                    return treeData;
                }
            }

            return null;
        }

        private void RegisterAdapters(ITabContext context)
        {
            //Add adapter to the pipeline as a ViewStatePageAdapter 

            context.Logger.Debug("Setting up view state page adapter");

            var adapters = HttpContext.Current.Request.Browser.Adapters;
            var keyType = typeof(Page);
            var adapterType = typeof(ViewStatePageAdapter);
            if (!adapters.Contains(keyType.AssemblyQualifiedName))
            {
                adapters.Add(keyType.AssemblyQualifiedName, adapterType.AssemblyQualifiedName);
            }
            else
            {
                var existingTypeString = (string)adapters[keyType.AssemblyQualifiedName];
                var existingType = Type.GetType(existingTypeString);
                if ((!existingType.IsGenericType && existingType != typeof(ViewStatePageAdapter)) || (existingType.IsGenericType && existingType.GetGenericTypeDefinition() != typeof(ViewStatePageAdapter<>)))
                {
                    var newAdapterType = typeof(ViewStatePageAdapter<>).MakeGenericType(existingType);
                    adapters[keyType.AssemblyQualifiedName] = newAdapterType.AssemblyQualifiedName;
                }
            }

            var dataBoundControlType = typeof(DataBoundControl);
            var dataBoundControlAdapterType = typeof(DataBoundControlAdapter);
            if (HttpContext.Current.Items["_GlimpseWebFormDataBinding"] == null)
            {
                HttpContext.Current.Items["_GlimpseWebFormDataBinding"] = new Dictionary<string, List<DataBindParameterModel>>();
            }
            foreach (DictionaryEntry adapter in adapters)
            {
                var controlType = Type.GetType((string)adapter.Key);
                if (typeof(DataBoundControl).IsAssignableFrom(controlType))
                {
                    var existingType = Type.GetType((string)adapter.Value);
                    if ((!existingType.IsGenericType && existingType != typeof(DataBoundControlAdapter)) || (existingType.IsGenericType && existingType.GetGenericTypeDefinition() != typeof(DataBoundControlAdapter<>)))
                    {
                        var newAdapterType = typeof(DataBoundControlAdapter<>).MakeGenericType(existingType);
                        adapters[adapter.Key] = newAdapterType.AssemblyQualifiedName;
                    }
                }
            }
            if (!adapters.Contains(dataBoundControlType.AssemblyQualifiedName))
            {
                adapters.Add(dataBoundControlType.AssemblyQualifiedName, dataBoundControlAdapterType.AssemblyQualifiedName);
            }

        }

        private object ProcessData(DataTable dataTable, ILogger logger, IEnumerable<PageLifeCycleMessage> pageLifeCycleMessages)
        {
            if (dataTable != null)
            {
                var controlList = new List<ControlTreeItemModel>();
                var nodeGraph = new ControlTreeItemTrackModel { ControlId = "ROOT", Indent = -1 };
                var nodeList = new Dictionary<string, ControlTreeItemTrackModel> { { "ROOT", nodeGraph } };

                logger.Debug("Start processing `Trace_Control_Tree`");

                var enumerator = dataTable.Rows.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = new ControlTreeItemModel();
                    var current = enumerator.Current as DataRow;

                    item.ParentControlId = current["Trace_Parent_Id"].CastOrDefault<string>();
                    item.ControlId = current["Trace_Control_Id"].CastOrDefault<string>();

                    var num = nodeList[item.ParentControlId].Indent + 1;
                    nodeList[item.ControlId] = new ControlTreeItemTrackModel
                    {
                        ParentControlId = item.ParentControlId,
                        ControlId = item.ControlId,
                        Indent = num,
                        Record = item
                    };
                    nodeList[item.ParentControlId].Children.Add(nodeList[item.ControlId]);

                    //This logic shouldn't be here
                    for (var index = 0; index < num; ++index)
                        item.ControlId = "\t" + item.ControlId;

                    item.Level = num;
                    item.Type = current["Trace_Type"].CastOrDefault<string>();
                    item.RenderSize = current["Trace_Render_Size"].CastOrDefault<int>();
                    item.ViewstateSize = current["Trace_Viewstate_Size"].CastOrDefault<int>();
                    item.ControlstateSize = current["Trace_Controlstate_Size"].CastOrDefault<int>();

                    controlList.Add(item);
                }

                logger.Debug("Finish processing `Trace_Control_Tree` - Count {0}", nodeList.Count);

                viewStateFormatter.Process(nodeGraph);
                dataBindFormatter.Process(nodeGraph, pageLifeCycleMessages);

                return controlList;
            }

            return null;
        }
    }
}
