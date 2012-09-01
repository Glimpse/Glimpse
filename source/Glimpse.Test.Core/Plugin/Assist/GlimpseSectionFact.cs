using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class GlimpseSectionFact
	{
		[Fact]
		public void GlimpseSection_New_HasNoRows()
		{
			var rows = Section.Rows.Count();

			Assert.Equal(0, rows);
		}

		[Fact]
		public void GlimpseSection_AddRow_AddsAndReturnsRow()
		{
			var row = Section.AddRow();

			var rows = Section.Rows;

			Assert.Equal(1, rows.Count());
			Assert.Equal(row, rows.First());
		}

		[Fact]
		public void GlimpseSection_Build_ReturnsRowsAsInstance()
		{
			Section.AddRow();

			var section = Section.Build();

			Assert.Equal(Section.Rows.Count(), section.Count());
			Assert.Equal(typeof(GlimpseSection.Instance), section.GetType());
		}

		[Fact]
		public void GlimpseSection_InstanceData_IsSelf()
		{
			var section = Section;
			var sectionInstance = Section.Build() as GlimpseSection.Instance;

			Assert.Equal(section, sectionInstance.Data);
		}

		[Fact]
		public void GlimpseSection_Instance_IsListOfObjectArray()
		{
			Assert.True(typeof(List<object[]>).IsAssignableFrom(typeof(GlimpseSection.Instance)));
		}

		private GlimpseSection Section { get; set; }

		public GlimpseSectionFact()
		{
			Section = new GlimpseSection();
		}
	}
}
