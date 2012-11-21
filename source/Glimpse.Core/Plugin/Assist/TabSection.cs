using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
    public class TabSection : ITabBuild
    {
        private readonly List<TabSectionRow> rows = new List<TabSectionRow>();

        public TabSection()
        {
        }

        public TabSection(params string[] headers)
        {
            if (headers.Any())
            {
                var row = AddRow();
                foreach (var header in headers)
                {
                    row.Column(header);
                }
            }
        }

        public IEnumerable<TabSectionRow> Rows
        {
            get { return rows; }
        }

        public TabSectionRow AddRow()
        {
            var row = new TabSectionRow();
            rows.Add(row);
            return row;
        }

        public object Build()
        { 
            return rows.Select(r => r.Build());
        } 
    }
}