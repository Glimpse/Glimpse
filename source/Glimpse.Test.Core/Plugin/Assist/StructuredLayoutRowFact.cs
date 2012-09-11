using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StructuredLayoutRowFact
	{
		[Fact]
		public void StructuredLayoutRow_New_HasNoCells()
		{
			var row = new StructuredLayoutRow();
			
			var cells = row.Build();

			Assert.Equal(0, cells.Count());
		}

		[Fact]
		public void StructuredLayoutRow_Cell_AddsSingleCell()
		{
			const int expectedCellId = 1;
			var row = new StructuredLayoutRow();
			var cell = row.Cell(expectedCellId);

			var cells = row.Build();

			Assert.Equal(1, cells.Count());
			Assert.Equal(cell, cells.First());
			Assert.Equal(cell.Data, expectedCellId);
		}

		[Fact]
		public void StructuredLayoutRow_Cell_AddsTwoCells()
		{
			var row = new StructuredLayoutRow();
			var cell1 = row.Cell(1);
			var cell2 = row.Cell(2);

			var cells = row.Build();

			Assert.Equal(2, cells.Count());
			Assert.Equal(cell1, cells.First());
			Assert.Equal(cell2, cells.Last());
		}

		[Fact]
		public void StructuredLayoutRow_Build_ReturnsObjectArrayOfColumnData()
		{
			var row = new StructuredLayoutRow();

			row.Cell(1);
			row.Cell(2);
			row.Cell(3);

			var cells = row.Build();

			Assert.Equal(3, cells.Count());
			Assert.Equal(1, cells.ElementAt(0).Data);
			Assert.Equal(2, cells.ElementAt(1).Data);
			Assert.Equal(3, cells.ElementAt(2).Data);
		}
	}
}