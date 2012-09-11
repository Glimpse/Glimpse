using System;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabRowShould
	{
		[Fact]
		public void HaveNoColumns()
		{
			var columns = Row.Columns;

			Assert.Equal(0, columns.Count());
		}

		[Fact]
		public void ThrowForNullValue()
		{
			object columnObject = null;
			
			Assert.Throws<ArgumentNullException>(() => Row.Column(columnObject));
		}

		[Fact]
		public void AddColumnAndReturnsSelf()
		{
			var columnObject = new { };
			var row = Row.Column(columnObject);

			Assert.Equal(Row, row);
			Assert.Equal(1, Row.Columns.Count());
		}

		[Fact]
		public void ReturnObjectArrayOfColumnData()
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

		public TabRowShould()
		{
			Row = new TabRow();
		}
	}
}
