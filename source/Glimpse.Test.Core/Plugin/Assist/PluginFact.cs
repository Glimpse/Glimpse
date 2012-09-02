using System;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class PluginFact
	{
		[Fact]
		public void Plugin_Create_ReturnsNewGlimpseSectionWithNoRows()
		{
			var section = Glimpse.Core.Plugin.Assist.Plugin.Create();

			Assert.Equal(0, section.Rows.Count());
		}

		[Fact]
		public void Plugin_Create_ReturnsNewGlimpseSectionWithRowsAndColumns()
		{
			var section = Glimpse.Core.Plugin.Assist.Plugin.Create("Header1", "Header2");
			
			Assert.Equal(1, section.Rows.Count());
			Assert.Equal(2, section.Rows.Single().Columns.Count());
			Assert.Equal("Header1", section.Rows.Single().Columns.First().Data);
			Assert.Equal("Header2", section.Rows.Single().Columns.Last().Data);
		}

		[Fact]
		public void Plugin_Section_ReturnsSelf()
		{
			var section1 = Plugin.Section("SectionName", new GlimpseSection());
			var section2 = Plugin.Section("SectionName", section => {});

			Assert.Equal(section1, Plugin);
			Assert.Equal(section2, Plugin);
		}

		[Fact]
		public void Plugin_Section_ThrowsWhenSectionNameIsNullOrEmpty()
		{
			var validSection = new GlimpseSection();
			Action<GlimpseSection> validSectionAction = section => {};
			
			Assert.Throws<ArgumentException>(() => Plugin.Section(null, validSection));
			Assert.Throws<ArgumentException>(() => Plugin.Section("", validSection));

			Assert.Throws<ArgumentException>(() => Plugin.Section(null, validSectionAction));
			Assert.Throws<ArgumentException>(() => Plugin.Section("", validSectionAction));
		}

		[Fact]
		public void Plugin_Section_ThrowsWhenSectionIsNull()
		{
			GlimpseSection section = null;
			Action<GlimpseSection> sectionAction = null;
			
			Assert.Throws<ArgumentNullException>(() => Plugin.Section("SectionName", section));
			Assert.Throws<ArgumentNullException>(() => Plugin.Section("SectionName", sectionAction));
		}

		[Fact]
		public void Plugin_Section_AddsNewSection()
		{
			var innerSection = new GlimpseSection();
			Plugin.Section("SectionName1", innerSection);

			Assert.Equal(1, Plugin.Rows.Count());
			Assert.Equal(2, Plugin.Rows.Single().Columns.Count());
			Assert.Equal("*SectionName1*", Plugin.Rows.Single().Columns.First().Data);
			Assert.Equal(innerSection.Build(), Plugin.Rows.Single().Columns.Last().Data);
		}

		[Fact]
		public void Plugin_Section_AddsSectionWithThreeRows()
		{
			object rows = null;
			Plugin.Section("SectionName2", innerSection =>
			{
				innerSection.AddRow();
				innerSection.AddRow();
				innerSection.AddRow();
				rows = innerSection.Build();
			});

			Assert.Equal(1, Plugin.Rows.Count());
			Assert.Equal(2, Plugin.Rows.Single().Columns.Count());
			Assert.Equal("*SectionName2*", Plugin.Rows.Single().Columns.First().Data);
			Assert.Equal(rows, Plugin.Rows.Single().Columns.Last().Data);
		}

		private GlimpseSection Plugin { get; set; }

		public PluginFact()
		{
			Plugin = Glimpse.Core.Plugin.Assist.Plugin.Create();
		}
	}
}