using System.Collections.Generic;

namespace Glimpse.Core.Plugin.Assist
{
	public class StructuredLayoutRow : List<StructuredLayoutCell>
	{
		public StructuredLayoutCell Cell(int cell)
		{
			var layoutCell = new StructuredLayoutCell(cell);
			Add(layoutCell);
			return layoutCell;
		}
	}
}