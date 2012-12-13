using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Tab.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabRowShould
	{
		[Fact]
		public void HaveNoColumns()
		{
			var columns = SectionRow.Columns;

			Assert.Equal(0, columns.Count());
		}

		[Fact]
		public void AcceptNullValue()
		{
			object columnObject = null;
			
			SectionRow.Column(columnObject);
		}

		[Fact]
		public void AddColumnAndReturnsSelf()
		{
			var columnObject = new { };
			var row = SectionRow.Column(columnObject);

			Assert.Equal(SectionRow, row);
			Assert.Equal(1, SectionRow.Columns.Count());
		}

		[Fact]
		public void ReturnObjectArrayOfColumnData()
		{
			var columnObject1 = new { Id = "obj1" };
			var columnObject2 = new { Id = "obj2" };

			SectionRow.Column(columnObject1).Column(columnObject2);

			SectionRow.Build();
            var columnData = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(2, SectionRow.Columns.Count());
			Assert.Equal(columnObject1, columnData.ElementAt(0));
			Assert.Equal(columnObject2, columnData.ElementAt(1));
		}

		private TabSectionRow SectionRow { get; set; }

		public TabRowShould()
		{
			SectionRow = new TabSectionRow();
		}
	}
}
