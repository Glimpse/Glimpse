using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class GlimpseColumnFact
	{
		[Fact]
		public void GlimpseColumn_New_HasData()
		{
			var column = Column;

			Assert.Equal(ColumnObject, column.Data);
		}

		[Fact]
		public void GlimpseColumn_New_HasTabSectionAsData()
		{
			var section = new TabSection();
			var column = new GlimpseColumn(section);

			Assert.Equal(section, column.Data.ToGlimpseSection());
		}

		[Fact]
		public void GlimpseColumn_OverrideData_SetsData()
		{
			var columnData = new { };
			var overrideColumnData = new { };
			var column = new GlimpseColumn(columnData);

			column.OverrideData(overrideColumnData);

			Assert.Equal(overrideColumnData, column.Data);
		}

		private object ColumnObject { get; set; }
		private GlimpseColumn Column { get; set; }

		public GlimpseColumnFact()
		{
			ColumnObject = new { SomeProperty = "SomeValue" };
			Column = new GlimpseColumn(ColumnObject);
		}
	}
}
