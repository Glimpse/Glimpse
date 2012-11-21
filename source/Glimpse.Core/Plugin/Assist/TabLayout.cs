using System;
using System.Collections.Generic;

namespace Glimpse.Core.Plugin.Assist
{
    using System.Linq;

    public class TabLayout
    {
        private readonly List<TabLayoutRow> rows = new List<TabLayoutRow>();

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

        public object Build()
        {
            return rows.Select(r => r.Build());
        }
    }
}