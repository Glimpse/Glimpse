using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Glimpse.WebForms.SerializationConverter;
using Glimpse.WebForms.Tab;

namespace Glimpse.WebForms.Support
{
    public class ViewStateFormatter
    {
        public void Process(ControlTreeItemTrackModel root)
        {
            var viewState = HttpContext.Current.Items["_GlimpseWebFormViewState"] as Pair;
            if (viewState != null && root.Children.Count > 0)
            {
                var innerViewstate = viewState.Second as Triplet;
                if (innerViewstate != null)
                {
                    ProcessRecord(root.Children[0], innerViewstate.Third);
                }
            }
        }

        private void ProcessRecord(ControlTreeItemTrackModel item, object viewstate)
        {
            if (viewstate != null)
            {
                var viewStateType = viewstate.GetType();
                if (viewStateType == typeof(Pair))
                {
                    var pair = (Pair)viewstate;
                    item.Record.Viewstate = ProcessData(item.Record.Type, pair.First);
                    ProcessRecord(item, pair.Second);
                }
                else if (viewStateType == typeof(Triplet))
                {
                    var triplet = (Triplet)viewstate;
                    item.Record.Viewstate = ProcessData(item.Record.Type, triplet.First);
                        //NEED TO DO SOMETHING WITH SECOND ITEM?
                    ProcessRecord(item, triplet.Third);
                }
                else if (viewStateType == typeof(ArrayList))
                {
                    var list = (ArrayList)viewstate;
                    for (var i = 0; i < list.Count; i = i + 2)
                    {
                        var index = (int)list[i];
                        ProcessRecord(item.Children[index], list[i + 1]);
                    }
                }
            }
        }

        private object ProcessData(string type, object data)
        {
            var result = (object)null;
            if (data != null)
            {
                if (type == "System.Web.UI.DataBoundLiteralControl")
                {
                    result = data;
                }
                else if (type == "System.Web.UI.WebControls.ListView")
                {
                    var list = (System.Object[])data;
                    result = ProcessData("System.Web.UI.WebControls.DataBoundControl", list[0]);
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
                        result = ProcessData("System.Web.UI.WebControls.WebControl", pair.First);
                        if (pair.Second != null)
                        {
                            var temp = new Dictionary<string, object>();
                            temp.Add("Base State", result);
                            temp.Add("Model Data Source State", ProcessData("System.Web.UI.WebControls.MethodParametersDictionary", pair.Second));
                            result = temp;
                        }
                    } 
                }
                else if (type == "System.Web.UI.WebControls.WebControl")
                {
                    var pair = data as Pair;
                    if (pair != null)
                    {
                        result = ProcessData("System.Web.UI.WebControls.Control", pair.First);
                        if (pair.Second != null)
                        {
                            var temp = new Dictionary<string, object>();
                            temp.Add("Base State", result);
                            temp.Add("Attribute State", ProcessData(type, pair.Second));
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
                else if (type == "System.Web.UI.WebControls.FormView")
                {
                    var list = data as IList;
                    if (list != null)
                    {
                        var temp = new Dictionary<string, object>();
                        temp.Add("Base State", ProcessData("System.Web.UI.WebControls.DataBoundControl", list[0]));
                        temp.Add("Pager Style", ProcessData("System.Web.UI.WebControls.FormView", list[1]));
                        temp.Add("Header Style", ProcessData("System.Web.UI.WebControls.FormView", list[2]));
                        temp.Add("Footer Style", ProcessData("System.Web.UI.WebControls.FormView", list[3]));
                        temp.Add("Row Style", ProcessData("System.Web.UI.WebControls.FormView", list[4]));
                        temp.Add("Edit Row Style", ProcessData("System.Web.UI.WebControls.FormView", list[5]));
                        temp.Add("Insert Row Style", ProcessData("System.Web.UI.WebControls.FormView", list[6]));
                        temp.Add("Bound Field Style", ProcessData("System.Web.UI.WebControls.FormView", list[7]));
                        temp.Add("Pager Settings", ProcessData("System.Web.UI.WebControls.FormView", list[8]));
                        temp.Add("Control Style Created", ProcessData("System.Web.UI.WebControls.FormView", list[9]));

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
                        var recordType = TypeManager.TryDeriveType(type);
                        if (recordType != null)
                        {
                            if (typeof(UserControl).IsAssignableFrom(recordType))
                            {
                                var pair = data as Pair;
                                if (pair != null && (pair.First != null || pair.Second != null))
                                {
                                    result = ProcessData(type + "_First", pair.First);
                                    if (pair.Second != null)
                                    {
                                        var temp = new Dictionary<string, object>();
                                        temp.Add("Base State", result);
                                        temp.Add("Attribute State", ProcessData(type + "_Second", pair.Second));
                                        result = temp;
                                    }
                                }
                            }
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
    }
}