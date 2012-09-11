using System;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabRowFact
	{
		[Fact]
		public void TabRow_New_HasNoColumns()
		{
			var columns = Row.Columns;

			Assert.Equal(0, columns.Count());
		}

		[Fact]
		public void TabRow_Column_ThrowsForNullValue()
		{
			object columnObject = null;
			
			Assert.Throws<ArgumentNullException>(() => Row.Column(columnObject));
		}

		[Fact]
		public void TabRow_Column_AddsColumnAndReturnsSelf()
		{
			var columnObject = new { };
			var row = Row.Column(columnObject);

			Assert.Equal(Row, row);
			Assert.Equal(1, Row.Columns.Count());
		}

		[Fact]
		public void TabRow_Build_ReturnsObjectArrayOfColumnData()
		{
			var columnObject1 = new { Id = "obj1" };
			var columnObject2 = new { Id = "obj2" };

			Row.Column(columnObject1).Column(columnObject2);

			var columnData = Row.Build();

			Assert.Equal(2, Row.Columns.Count());
			Assert.Equal(columnObject1, columnData.ElementAt(0));
			Assert.Equal(columnObject2, columnData.ElementAt(1));
		}

		private TabRow Row { get; set; }

		public TabRowFact()
		{
			Row = new TabRow();
		}
	}
}
