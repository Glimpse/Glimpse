using System.Collections.Generic;
using Glimpse.WebForms.Model;

namespace Glimpse.WebForms.Tab
{
    public class ControlTreeItemTrackModel
    {
        public ControlTreeItemTrackModel()
        {
            Children = new List<ControlTreeItemTrackModel>();
        }

        public IList<ControlTreeItemTrackModel> Children { get; set; }

        public string ParentControlId { get; set; }

        public string ControlId { get; set; }

        public int Indent { get; set; }

        public ControlTreeItemModel Record { get; set; }
    }
}