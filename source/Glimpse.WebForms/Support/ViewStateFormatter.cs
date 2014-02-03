using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.WebForms.SerializationConverter;
using Glimpse.WebForms.Tab;

namespace Glimpse.WebForms.Support
{
    public class ViewStateFormatter
    {
        private ILogger logger;

        protected ILogger Logger
        {
            get { return logger ?? (logger = GlimpseConfiguration.GetLogger()); } 
        }

        public void Process(ControlTreeItemTrackModel root)
        {
            Logger.Debug("Start merginging in the viewState data to the ControlTree");

            var viewState = HttpContext.Current.Items["_GlimpseWebFormViewState"] as Pair;
            var controlTypes = HttpContext.Current.Items["_GlimpseWebFormControlTreeType"] as Dictionary<string, Type>;

            if (viewState != null && root.Children.Count > 0)
            { 
                Logger.Debug("Process Rroot node for viewState data to the ControlTree mapping");
                ProcessRecord(root.Children[0], viewState, controlTypes); 
            }

            Logger.Debug("Finish merginging in the viewState data to the ControlTree");
        }

        private void ProcessRecord(ControlTreeItemTrackModel item, object viewstate, Dictionary<string, Type> controlTypes)
        {
            if (viewstate != null)
            {
                var viewStateType = viewstate.GetType();
                if (viewStateType == typeof(Pair))
                {
                    var pair = (Pair)viewstate;
                    item.Record.Viewstate = ProcessData(controlTypes[item.ControlId], item.Record.Type, pair.First);
                    ProcessRecord(item, pair.Second, controlTypes);
                }
                else if (viewStateType == typeof(Triplet))
                {
                    var triplet = (Triplet)viewstate;
                    item.Record.Viewstate = ProcessData(controlTypes[item.ControlId], item.Record.Type, triplet.First);
                        //NEED TO DO SOMETHING WITH SECOND ITEM?
                    ProcessRecord(item, triplet.Third, controlTypes);
                }
                else if (viewStateType == typeof(ArrayList))
                {
                    var list = (ArrayList)viewstate;
                    for (var i = 0; i < list.Count; i = i + 2)
                    {
                        var index = (int)list[i];
                        ProcessRecord(item.Children[index], list[i + 1], controlTypes);
                    }
                }
            }
        }

        private object ProcessData(Type rootType, string type, object data)
        {
            var result = (object)null;
            if (data != null)
            {
                //My life is sad
                if (type == "System.Web.UI.DataBoundLiteralControl")
                {
                    result = data;
                }
                else if (type == "System.Web.UI.WebControls.ListView")
                {
                    var list = (Object[])data;
                    result = ProcessData(rootType, "System.Web.UI.WebControls.DataBoundControl", list[0]);
                    if (list[1] != null)
                    {
                        var temp = new Dictionary<object, object>();
                        temp.Add("Base State", result);
                        temp.Add("Bound Field Values State", list[1]);

                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.DataBoundControl")
                {
                    var pair = data as Pair;
                    if (pair != null)
                    {
                        result = ProcessData(rootType, "System.Web.UI.WebControls.WebControl", pair.First);
                        if (pair.Second != null)
                        {
                            var temp = new Dictionary<string, object>();
                            temp.Add("Base State", result);
                            temp.Add("Model Data Source State", ProcessData(rootType, "System.Web.UI.WebControls.MethodParametersDictionary", pair.Second));
                            result = temp;
                        }
                    } 
                }
                else if (data is Pair && (type == "System.Web.UI.WebControls.WebControl" || type == "System.Web.UI.UserControl"))
                { 
                    var pair = data as Pair;
                    if (pair != null && (pair.First != null || pair.Second != null))
                    {
                        result = WrapProcessedData(pair.First);
                        if (pair.Second != null)
                        {
                            var temp = new Dictionary<string, object>();
                            temp.Add("Base State", result);
                            temp.Add("Attribute State", WrapProcessedData(pair.Second));
                            result = temp; 
                        }
                    }
                }
                else if (type == "System.Web.UI.WebControls.MethodParametersDictionary")
                {
                    var pair = data as Pair;
                    if (pair != null)
                    {
                        var keysList = pair.First as IList;
                        var valuesList = pair.Second as IList;
                        if (keysList != null && valuesList != null)
                        {
                            var temp = new Dictionary<object, object>();
                            for (int i = 0; i < keysList.Count; i++)
                            {
                                temp.Add(keysList[i], new IndexedStringListConverterTarget(valuesList[i]));
                            }

                            result = temp;
                        }
                    }
                }
                else if (type == "System.Web.UI.WebControls.CheckBox" || type == "System.Web.UI.WebControls.RadioButton")
                {
                    var triplet = data as Triplet;
                    if (triplet != null)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.WebControl", triplet.First));
                        temp.Add("Input Attributes State", triplet.Second);
                        temp.Add("Label Attributes State", triplet.Third);
                        result = temp;
                    }
                    
                }
                else if (type == "System.Web.UI.WebControls.Repeater")
                {
                    var pair = data as Pair;
                    if (pair != null)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", pair.First);
                        temp.Add("Model Data Source State", ProcessData(rootType, "System.Web.UI.WebControls.ModelDataSource", pair.Second)); 
                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.ModelDataSource")
                {
                    result = ProcessData(rootType, "System.Web.UI.WebControls.ModelDataSourceView", data);
                }
                else if (type == "System.Web.UI.WebControls.ModelDataSourceView")
                {
                    result = ProcessData(rootType, "System.Web.UI.WebControls.MethodParametersDictionary", data);
                }
                else if (type == "System.Web.UI.WebControls.ListControl" || type == "System.Web.UI.WebControls.BulletedList")
                {
                    var triplet = data as Triplet;
                    if (triplet != null)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.DataBoundControl", triplet.First));
                        temp.Add("Items State", ProcessData(rootType, "System.Web.UI.WebControls.ListItemCollection", triplet.Second));
                        temp.Add("Selected Indices", triplet.Third);
                        result = temp;

                    }
                }
                else if (type == "System.Web.UI.WebControls.ListItemCollection")
                {
                    var triplet = data as Triplet;
                    if (triplet != null)
                    {
                        var temp = new List<object>();
                        var textList = triplet.First as object[];
                        var valueList = triplet.Second as object[];
                        var flagList = triplet.Third as bool[];

                        if (textList != null && valueList != null && flagList != null)
                        {
                            for (int i = 0; i < textList.Length; i++)
                            {
                                temp.Add(new { Text = textList[i], Value = valueList[i], Enabled = flagList[i] });
                            }

                            result = temp;
                        }
                        else
                        {
                            result = triplet;
                        } 
                    }
                    else
                    {
                        var pair = data as Pair;
                        if (pair != null)
                        {
                            var temp = new List<object>();
                            var indexList = pair.First as ArrayList;
                            var valueList = pair.Second as ArrayList;

                            if (indexList != null && valueList != null)
                            {
                                for (int i = 0; i < indexList.Count; i++)
                                {
                                    temp.Add(ProcessData(rootType, "System.Web.UI.WebControls.ListItem", new TempListItem { Index = indexList[i], Data = valueList[i] }));
                                }

                                result = temp;
                            }
                            else
                            {
                                result = pair;
                            } 
                        }
                    }
                }
                else if (type == "System.Web.UI.WebControls.ListItem")
                {
                    var payload = data as TempListItem;
                    if (payload != null)
                    {
                        var triplet = payload.Data as Triplet;
                        if (triplet != null)
                        {
                            result = new { Index = payload.Index, Text = triplet.First, Value = triplet.Second, Enabled = triplet.Third };
                        }
                        else
                        {
                            var pair = payload.Data as Pair;
                            result = pair != null ? new { Index = payload.Index, Text = pair.First, Value = pair.Second } : data;
                        }
                    }
                }
                else if (type == "System.Web.UI.WebControls.CompositeDataBoundControl" || type == "System.Web.UI.WebControls.AdRotator")
                {
                    result = ProcessData(rootType, "System.Web.UI.WebControls.DataBoundControl", data);
                }
                else if (type == "System.Web.UI.WebControls.FormView")
                { 
                    var list = data as IList;
                    if (list != null && list.Count == 10)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.CompositeDataBoundControl", list[0]));
                        temp.Add("Pager Style", WrapProcessedData(list[1]));
                        temp.Add("Header Style", WrapProcessedData(list[2]));
                        temp.Add("Footer Style", WrapProcessedData(list[3]));
                        temp.Add("Row Style", WrapProcessedData(list[4]));
                        temp.Add("Edit Row Style", WrapProcessedData(list[5]));
                        temp.Add("Insert Row Style", WrapProcessedData(list[6]));
                        temp.Add("Bound Field Style", ProcessData(rootType, "System.Web.UI.OrderedDictionaryStateHelper", list[7]));
                        temp.Add("Pager Settings", WrapProcessedData(list[8]));
                        temp.Add("Control Style Created", WrapProcessedData(list[9]));

                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.DetailsView") 
                { 
                    var list = data as object[]; 
                    if (list != null && list.Length == 15)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.CompositeDataBoundControl", list[0]));
                        temp.Add("Pager Style", WrapProcessedData(list[1]));
                        temp.Add("Header Style", WrapProcessedData(list[2]));
                        temp.Add("Footer Style", WrapProcessedData(list[3]));
                        temp.Add("Row Style", WrapProcessedData(list[4]));
                        temp.Add("Alternating Row Style", WrapProcessedData(list[5]));
                        temp.Add("Command Row Style", WrapProcessedData(list[6]));
                        temp.Add("Edit Row Style", WrapProcessedData(list[7]));
                        temp.Add("Insert Row Style", WrapProcessedData(list[8]));
                        temp.Add("Field Header Style", WrapProcessedData(list[9]));
                        temp.Add("Field Collection", list[10]);  //Could do more here 
                        temp.Add("Bound Field Style", ProcessData(rootType, "System.Web.UI.OrderedDictionaryStateHelper", list[11])); 
                        temp.Add("Pager Settings", WrapProcessedData(list[12]));
                        temp.Add("Control Style", WrapProcessedData(list[13]));
                        temp.Add("Rows Generator", list[14]); //Could do more here 

                        result = temp;
                    } 
                }
                else if (type == "System.Web.UI.WebControls.GridView")
                { 
                    var list = data as object[];
                    if (list != null && list.Length == 16)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.CompositeDataBoundControl", list[0]));
                        temp.Add("Field Collection", list[1]); //Could do more here 
                        temp.Add("Pager Style", WrapProcessedData(list[2]));
                        temp.Add("Header Style", WrapProcessedData(list[3]));
                        temp.Add("Footer Style", WrapProcessedData(list[4]));
                        temp.Add("Row Style", WrapProcessedData(list[5]));
                        temp.Add("Alternating Row Style", WrapProcessedData(list[6]));
                        temp.Add("Select Row Style", WrapProcessedData(list[7]));
                        temp.Add("Edit Row Style", WrapProcessedData(list[8]));
                        temp.Add("Bound Field Values", ProcessData(rootType, "System.Web.UI.OrderedDictionaryStateHelper", list[9]));
                        temp.Add("Pager Settings", WrapProcessedData(list[10]));
                        temp.Add("Control Style Created", WrapProcessedData(list[11]));
                        temp.Add("Columns Generator", list[12]); //Could do more here 
                        temp.Add("Sorted Ascending Cell Style", WrapProcessedData(list[13]));
                        temp.Add("Sorted Descending Cell Style", WrapProcessedData(list[14]));
                        temp.Add("Sorted Ascending Header Style", WrapProcessedData(list[15]));
                        temp.Add("Sorted Descending Header Style", WrapProcessedData(list[16]));

                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.RadioButtonList" || type == "System.Web.UI.WebControls.BulletedList" || type == "System.Web.UI.WebControls.CheckBoxList" || type == "System.Web.UI.WebControls.DropDownList" || type == "System.Web.UI.WebControls.ListBox")
                {
                    result = ProcessData(rootType, "System.Web.UI.WebControls.ListControl", data);
                }
                else if (type == "System.Web.UI.WebControls.Menu")
                {
                    var list = data as object[];
                    if (list != null && list.Length == 13)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.WebControls", list[0]));
                        temp.Add("Static Item Style", WrapProcessedData(list[1]));
                        temp.Add("Static Selected Style", WrapProcessedData(list[2]));
                        temp.Add("Static Hover Style", WrapProcessedData(list[3]));
                        temp.Add("Static Menu Style", WrapProcessedData(list[4]));
                        temp.Add("Dynamic Item Style", WrapProcessedData(list[5]));
                        temp.Add("Dynamic Selected Style", WrapProcessedData(list[6]));
                        temp.Add("Dynamic Hover Style", WrapProcessedData(list[7]));
                        temp.Add("Dynamic Menu Style", WrapProcessedData(list[8]));
                        temp.Add("Level Menu Item Styles", WrapProcessedData(list[9]));
                        temp.Add("Level Selected Styles", WrapProcessedData(list[10]));
                        temp.Add("Level Styles", WrapProcessedData(list[11]));
                        temp.Add("Items", list[12]); //Could do more here 

                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.TreeView")
                {
                    var list = data as object[];
                    if (list != null && list.Length == 9)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.WebControls", list[0]));
                        temp.Add("Node Style", WrapProcessedData(list[1]));
                        temp.Add("Root Node Style", WrapProcessedData(list[2]));
                        temp.Add("Parent Node Style", WrapProcessedData(list[3]));
                        temp.Add("Leaf Node Style", WrapProcessedData(list[4]));
                        temp.Add("Selected Node Style", WrapProcessedData(list[5]));
                        temp.Add("Hover Node Style", WrapProcessedData(list[6]));
                        temp.Add("Level Style", WrapProcessedData(list[7]));
                        temp.Add("Nodes", list[8]); //Could do more here 

                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.DataGrid")
                { 
                    var list = data as object[];
                    if (list != null && list.Length == 11)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.WebControls", list[0]));
                        temp.Add("Columns", ProcessData(rootType, "DataGridColumnCollection", list[1]));
                        temp.Add("Pager Style", WrapProcessedData(list[2]));
                        temp.Add("Header Style", WrapProcessedData(list[3]));
                        temp.Add("Footer Style", WrapProcessedData(list[4]));
                        temp.Add("Item Style", WrapProcessedData(list[5]));
                        temp.Add("Alternating Item Style", WrapProcessedData(list[6]));
                        temp.Add("Selected Item Style", WrapProcessedData(list[7]));
                        temp.Add("Edit Item Style", WrapProcessedData(list[8]));
                        temp.Add("Control Style", WrapProcessedData(list[9]));
                        temp.Add("Auto Generated Columns", list[10]); //Could do more here 

                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.DataGridColumnCollection")
                {
                    var list = data as object[];
                    if (list != null)
                    {
                        var temp = new List<object>();
                        for (int i = 0; i < list.Length; i++)
                        {
                            var item = list[i] as object[];

                            temp.Add(new { Item = WrapProcessedData(item[0]), ItemStyle = WrapProcessedData(item[1]), HeaderStyle = WrapProcessedData(item[2]), FooterStyle = WrapProcessedData(item[3]) });
                        }

                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.DataList")
                { 
                    var list = data as object[];
                    if (list != null && list.Length == 9)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData(rootType, "System.Web.UI.WebControls.WebControls", list[0])); 
                        temp.Add("Item Style", WrapProcessedData(list[1]));
                        temp.Add("Selected Item Style", WrapProcessedData(list[2]));
                        temp.Add("Alternating Item Style", WrapProcessedData(list[3]));
                        temp.Add("Edit Item Style", WrapProcessedData(list[4]));
                        temp.Add("Separator Style", WrapProcessedData(list[5]));
                        temp.Add("Header Style", WrapProcessedData(list[6]));
                        temp.Add("Footer Style", WrapProcessedData(list[7]));
                        temp.Add("Control Style", WrapProcessedData(list[8])); 

                        result = temp;
                    } 
                }
                else if (type == "System.Web.UI.OrderedDictionaryStateHelper")
                {
                    var list = data as ArrayList;
                    if (list != null)
                    {
                        var temp = new Dictionary<object, object>();
                        foreach (var item in list)
                        {
                            var pair = item as Pair; 
                            temp.Add(pair.First, pair.Second);  
                        }
                    }
                }
                else if (type == "System.Web.UI.WebControls.ObjectDataSource" || type == "System.Web.UI.WebControls.LinqDataSource" || type == "System.Web.UI.WebControls.SqlDataSource")
                {
                    var pair = data as Pair;
                    if (pair != null)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", WrapProcessedData(pair.First));
                        temp.Add("Object Data Source View", ProcessData(rootType, type + "View", pair.Second));
                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.ObjectDataSourceView" || type == "System.Web.UI.WebControls.SqlDataSourceView")
                {
                    var pair = data as Pair;
                    if (pair != null)
                    {
                        //TODO: Try and break down this object structure
                        var temp = new Dictionary<string, object>();
                        temp.Add("Select Parameters", pair.First);
                        temp.Add("Filter Parameters", pair.Second);
                        result = temp;
                    }
                }
                else if (type == "System.Web.UI.WebControls.LinqDataSourceView")
                {
                    var array = data as object[];
                    if (array != null)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Where Parameters", array[0]);
                        temp.Add("OrderBy Parameters", array[1]);
                        temp.Add("GroupBy Parameters", array[2]);
                        temp.Add("Order GroupsBy Parameters", array[3]);
                        temp.Add("SelectNew Parameters", array[4]);
                        temp.Add("Original Values", array[5]);

                        result = temp;
                    }
                }

                if (result == null)
                {
                    if (typeof(IList).IsAssignableFrom(data.GetType()))
                    {
                        result = new IndexedStringListConverterTarget(data);
                    }

                    if (result == null)
                    {
                        if (typeof(System.Web.UI.UserControl).IsAssignableFrom(rootType))
                        {
                            result = ProcessData(rootType, "System.Web.UI.UserControl", data);
                        }
                        else if (typeof(System.Web.UI.WebControls.WebControl).IsAssignableFrom(rootType) && rootType.FullName.Contains("System.Web.UI.WebControls."))
                        {
                            result = ProcessData(rootType, "System.Web.UI.WebControls.WebControl", data);
                        }
                    }
                }

                if (result == null)
                {
                    result = data;
                }
            }

            return result;
        }

        private object WrapProcessedData(object data)
        {
            return data == null ? data : new IndexedStringListConverterTarget(data);
        }


        private class TempListItem
        {
            public object Index { get; set; }

            public object Data { get; set; } 
        }
    }
}