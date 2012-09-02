using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StructuredLayoutRowFact
	{
		[Fact]
		public void StructuredLayoutRow_New_HasCells()
		{
			var row = new StructuredLayoutRow();
			
			var cells = row.Count();
			
			Assert.Equal(0, cells);
		}

		[Fact]
		public void StructuredLayoutRow_New_HasSingleCell()
		{
			var cell = new StructuredLayoutCell(1);
			var row = new StructuredLayoutRow { cell };

			var cells = row;

			Assert.Equal(1, cells.Count);
			Assert.Equal(cell, cells.First());
		}

		[Fact]
		public void StructuredLayoutRow_Cell_AddsSingleCell()
		{
			const int expectedCellId = 1;
			var row = new StructuredLayoutRow();
			var cell = row.Cell(expectedCellId);

			var cells = row;

			Assert.Equal(1, cells.Count);
			Assert.Equal(cell, cells.First());
			Assert.Equal(cell.Data, expectedCellId);
		}

		[Fact]
		public void StructuredLayoutRow_Cell_AddsTwoCells()
		{
			var row = new StructuredLayoutRow();
			var cell1 = row.Cell(1);
			var cell2 = row.Cell(2);

			var cells = row;

			Assert.Equal(2, cells.Count);
			Assert.Equal(cell1, cells.First());
			Assert.Equal(cell2, cells.Last());
		}
	}
}