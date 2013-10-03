using System;
using System.Collections.Generic;
using System.Data; 
using System.Reflection; 
using System.Web;
using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Extensions;
using Glimpse.Core.Tab.Assist;
using Glimpse.WebForms.Model;

namespace Glimpse.WebForms.Tab
{
    public class ControlTree : AspNetTab, ITabLayout, IKey
    {
        private static readonly object Layout = TabLayout.Create()
                .Row(r =>
                {
                    r.Cell("controlId").AsKey().WithTitle("Control ID");
                    r.Cell("type").WithTitle("Type");
                    r.Cell("renderSize").Class("mono").AlignRight().WidthInPixels(150).WithTitle("Render (w/ children)").Suffix(" Bytes");
                    r.Cell("viewstateSize").Class("mono").AlignRight().WidthInPixels(125).WithTitle("ViewState").Suffix(" Bytes");
                    r.Cell("controlstateSize").Class("mono").AlignRight().WidthInPixels(125).WithTitle("ControlState").Suffix(" Bytes");
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

        public override object GetData(ITabContext context)
        { 
            var trace = HttpContext.Current.Trace;

            var hasRun = context.TabStore.Get("hasRun");
            if (hasRun == null)
            {
                context.TabStore.Set("hasRun", "true");
                 
                trace.IsEnabled = true;
                trace.TraceFinished += (sender, args) => trace.IsEnabled = false;  

                return null;
            }

            var requestDataField = typeof(System.Web.TraceContext).GetField("_requestData", BindingFlags.Instance | BindingFlags.NonPublic);
            if (requestDataField != null) 
            { 
                var requestData = requestDataField.GetValue(trace) as DataSet;
                if (requestData != null)
                {
                    var treeData = ProcessData(requestData.Tables["Trace_Control_Tree"]);
                    return treeData;
                }
            }
             
            return null;
        }

        private object ProcessData(DataTable dataTable)
        {
            if (dataTable != null)
            {
                var result = new List<ControlTreeItem>();
                var treeTracker = new Dictionary<string, int>();

                var enumerator = dataTable.Rows.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var item = new ControlTreeItem();
                    var current = enumerator.Current as DataRow;

                    item.ParentControlId = current["Trace_Parent_Id"].CastOrDefault<string>();
                    item.ControlId = current["Trace_Control_Id"].CastOrDefault<string>();

                    var num = treeTracker.GetValueOrDefault(item.ParentControlId);
                    treeTracker[item.ControlId] = num + 1;

                    //This logic shouldn't be here
                    for (var index = 0; index < num; ++index)
                        item.ControlId = "\t" + item.ControlId; 

                    item.Level = num; 
                    item.Type = current["Trace_Type"].CastOrDefault<string>(); 
                    item.RenderSize = current["Trace_Render_Size"].CastOrDefault<int>();
                    item.ViewstateSize = current["Trace_Viewstate_Size"].CastOrDefault<int>(); 
                    item.ControlstateSize = current["Trace_Controlstate_Size"].CastOrDefault<int>(); 
                    
                    result.Add(item);
                }

                return result;
            }

            return null;
        } 
    }
}
