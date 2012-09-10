using System.Collections.Generic;

namespace Glimpse.Core.Plugin.Assist
{
	public class StructuredLayoutRow
	{
		private readonly List<StructuredLayoutCell> _cells = new List<StructuredLayoutCell>();

		public StructuredLayoutCell Cell(int cell)
		{
			var layoutCell = new StructuredLayoutCell(cell);
			_cells.Add(layoutCell);
			return layoutCell;
		}

		internal StructuredLayoutCell[] Build()
		{
			return _cells.ToArray();
		}
	}
}