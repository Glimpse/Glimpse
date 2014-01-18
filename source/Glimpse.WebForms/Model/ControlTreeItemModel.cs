using System.Collections.Generic;
namespace Glimpse.WebForms.Model
{
    public class ControlTreeItemModel
    {
        public string ParentControlId { get; set; }

        public string ControlId { get; set; }

        public string Type { get; set; }

        public int? RenderSize { get; set; }

        public int? ViewstateSize { get; set; }

        public int? ControlstateSize { get; set; }

        public int Level { get; set; }

        public object Viewstate { get; set; }

        public string ViewstateTitle
        {
            get { return Viewstate != null ? "Viewstate" : null; }
        }

        public List<DataBindParameterModel> DataBindParameters { get; set; }

        public string DataBindParametersTitle
        {
            get { return DataBindParameters != null ? "DataBind Parameters" : null; }
        }
    }
}