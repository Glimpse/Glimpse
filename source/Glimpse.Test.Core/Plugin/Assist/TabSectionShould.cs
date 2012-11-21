using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabSectionShould
	{
		[Fact]
		public void HaveNoRows()
		{
			var rows = Section.Rows.Count();

			Assert.Equal(0, rows);
		}

		[Fact]
		public void AddAndReturnsRow()
		{
			var row = Section.AddRow();

			var rows = Section.Rows;

			Assert.Equal(1, rows.Count());
			Assert.Equal(row, rows.First());
		}

		private TabSection Section { get; set; }

		public TabSectionShould()
		{
			Section = new TabSection();
		}
	}
}
