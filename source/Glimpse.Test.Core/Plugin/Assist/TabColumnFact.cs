using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabColumnFact
	{
		[Fact]
		public void TabColumn_New_HasData()
		{
			var column = Column;

			Assert.Equal(ColumnObject, column.Data);
		}

		[Fact]
		public void TabColumn_New_HasTabSectionAsData()
		{
			var section = new TabSection();
			var column = new TabColumn(section);

			Assert.Equal(section, column.Data.ToGlimpseSection());
		}

		[Fact]
		public void TabColumn_OverrideData_SetsData()
		{
			var columnData = new { };
			var overrideColumnData = new { };
			var column = new TabColumn(columnData);

			column.OverrideData(overrideColumnData);

			Assert.Equal(overrideColumnData, column.Data);
		}

		private object ColumnObject { get; set; }
		private TabColumn Column { get; set; }

		public TabColumnFact()
		{
			ColumnObject = new { SomeProperty = "SomeValue" };
			Column = new TabColumn(ColumnObject);
		}
	}
}
