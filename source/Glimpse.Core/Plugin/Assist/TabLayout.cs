using System;
using System.Collections.Generic;

namespace Glimpse.Core.Plugin.Assist
{
	public class TabLayout
	{
		private readonly List<TabLayoutRow> _rows = new List<TabLayoutRow>();

		public IEnumerable<TabLayoutRow> Rows
		{
			get { return _rows; }
		}

		private TabLayout() {}

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
			_rows.Add(layoutRow);
			return this;
		}
	}
}