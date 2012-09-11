using System;
using System.Collections.Generic;

namespace Glimpse.Core.Plugin.Assist
{
	public class StructuredLayout
	{
		private readonly List<StructuredLayoutRow> _rows = new List<StructuredLayoutRow>();

		public IEnumerable<StructuredLayoutRow> Rows
		{
			get { return _rows; }
		}

		private StructuredLayout() {}

		public static StructuredLayout Create()
		{
			return new StructuredLayout();
		}

		public static StructuredLayout Create(Action<StructuredLayout> layout)
		{
			var structuredLayout = new StructuredLayout();
			layout.Invoke(structuredLayout);
			return structuredLayout;
		}

		public StructuredLayout Row(Action<StructuredLayoutRow> row)
		{
			var layoutRow = new StructuredLayoutRow();
			row.Invoke(layoutRow);
			_rows.Add(layoutRow);
			return this;
		}
	}
}