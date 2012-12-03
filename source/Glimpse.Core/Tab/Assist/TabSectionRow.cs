using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Tab.Assist
{
    public class TabSectionRow : ITabBuild, ITabStyleValue<TabSectionRow>, ITabStyleRow
    {
        private readonly List<TabSectionColumn> columns = new List<TabSectionColumn>();

        // TODO this might be able to go
        public IEnumerable<TabSectionColumn> Columns
        {
            get { return columns; }
        }

        public TabSectionRow Column(object columnData)
        { 
            var column = new TabSectionColumn(columnData);
            columns.Add(column);
            
            return this;
        }

        public object Build()
        {
            return columns.Select(x => x.Build());
        }

        public TabSectionRow ApplyValueStyle(string format)
        {
            var coloum = columns.Last();
            var formattedData = format.FormatWith(coloum.Data);
            coloum.OverrideData(formattedData);

            return this;
        }

        public void ApplyRowStyle(string style)
        {
            Column(style);
        }
    }
}