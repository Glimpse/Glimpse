using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabColumnShould
	{
		[Fact]
		public void ConstructWithData()
		{
			var column = Column;

			Assert.Equal(ColumnObject, column.Data);
		}

		[Fact]
		public void SetData()
		{
			var columnData = new { };
			var overrideColumnData = new { };
			var column = new TabColumn(columnData);

			column.OverrideData(overrideColumnData);

			Assert.Equal(overrideColumnData, column.Data);
		}

		private object ColumnObject { get; set; }
		private TabColumn Column { get; set; }

		public TabColumnShould()
		{
			ColumnObject = new { SomeProperty = "SomeValue" };
			Column = new TabColumn(ColumnObject);
		}
	}
}
