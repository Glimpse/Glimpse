using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Tab.Assist
{
    public class TabLayout : ITabBuild
    {
        private readonly List<TabLayoutRow> rows = new List<TabLayoutRow>();
        private readonly Dictionary<string, TabLayout> cells = new Dictionary<string, TabLayout>();

        private TabLayout()
        {
        }

        public IEnumerable<TabLayoutRow> Rows
        {
            get { return rows; }
        }

        public static TabLayout Create()
        {
            return new TabLayout();
        }

        public static TabLayout Create(Action<TabLayout> layout)
        {
            var tabLayout = new TabLayout();
            layout.Invoke(tabLayout);
            return tabLayout;
        }

        public TabLayout Row(Action<TabLayoutRow> row)
        {
            var layoutRow = new TabLayoutRow();
            row.Invoke(layoutRow);
            rows.Add(layoutRow);
            return this;
        }

        public TabLayout Cell(string target, TabLayout layout)
        {
            cells.Add(target, layout);
            return this;
        }

        public object Build()
        {
            if (cells.Count > 0)
            {
                return cells.ToDictionary(x => x.Key, x => new { Layout = x.Value.Build() });
            }

            return rows.Select(r => r.Build());
        }
    }
}