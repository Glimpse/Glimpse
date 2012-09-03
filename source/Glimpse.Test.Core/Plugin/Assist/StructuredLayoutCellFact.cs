using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StructuredLayoutCellFact
	{
		[Fact]
		public void StructuredLayoutCell_New_HasDataId()
		{
			var cell = new StructuredLayoutCell(1);
			
			Assert.Equal(1, cell.Data);
		}

		[Fact]
		public void StructuredLayoutCell_New_HasDataFormat()
		{
			var cell = new StructuredLayoutCell("{0} - {1}");

			Assert.Equal("{0} - {1}", cell.Data);
		}
	}
}