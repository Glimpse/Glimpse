using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabSectionFact
	{
		[Fact]
		public void TabSection_New_HasNoRows()
		{
			var rows = Section.Rows.Count();

			Assert.Equal(0, rows);
		}

		[Fact]
		public void TabSection_AddRow_AddsAndReturnsRow()
		{
			var row = Section.AddRow();

			var rows = Section.Rows;

			Assert.Equal(1, rows.Count());
			Assert.Equal(row, rows.First());
		}

		[Fact]
		public void TabSection_Build_ReturnsRowsAsInstance()
		{
			Section.AddRow();

			var section = Section.Build();

			Assert.Equal(Section.Rows.Count(), section.Count());
			Assert.Equal(typeof(TabSection.Instance), section.GetType());
		}

		[Fact]
		public void TabSection_InstanceData_IsSelf()
		{
			var section = Section;
			var sectionInstance = Section.Build() as TabSection.Instance;

			Assert.Equal(section, sectionInstance.Data);
		}

		[Fact]
		public void TabSection_Instance_IsListOfObjectArray()
		{
			Assert.True(typeof(List<object[]>).IsAssignableFrom(typeof(TabSection.Instance)));
		}

		private TabSection Section { get; set; }

		public TabSectionFact()
		{
			Section = new TabSection();
		}
	}
}
