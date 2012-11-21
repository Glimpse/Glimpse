using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class PluginShould
	{
		[Fact]
		public void ReturnNewSectionWithNoRows()
		{
			var section = Glimpse.Core.Plugin.Assist.Plugin.Create();

			Assert.Equal(0, section.Rows.Count());
		}

		[Fact]
		public void ReturnNewSectionWithRowsAndColumns()
		{
			var section = Glimpse.Core.Plugin.Assist.Plugin.Create("Header1", "Header2");
			
			Assert.Equal(1, section.Rows.Count());
			Assert.Equal(2, section.Rows.Single().Columns.Count());
			Assert.Equal("Header1", section.Rows.Single().Columns.First().Data);
			Assert.Equal("Header2", section.Rows.Single().Columns.Last().Data);
		}

		[Fact]
		public void ReturnSelf()
		{
			var section1 = Plugin.Section("SectionName", new TabSection());
			var section2 = Plugin.Section("SectionName", section => {});

			Assert.Equal(section1, Plugin);
			Assert.Equal(section2, Plugin);
		}

		[Fact]
		public void ThrowWhenSectionNameIsNullOrEmpty()
		{
			var validSection = new TabSection();
			Action<TabSection> validSectionAction = section => {};
			
			Assert.Throws<ArgumentException>(() => Plugin.Section(null, validSection));
			Assert.Throws<ArgumentException>(() => Plugin.Section("", validSection));

			Assert.Throws<ArgumentException>(() => Plugin.Section(null, validSectionAction));
			Assert.Throws<ArgumentException>(() => Plugin.Section("", validSectionAction));
		}

		[Fact]
		public void ThrowWhenSectionIsNull()
		{
			TabSection section = null;
			Action<TabSection> sectionAction = null;
			
			Assert.Throws<ArgumentNullException>(() => Plugin.Section("SectionName", section));
			Assert.Throws<ArgumentNullException>(() => Plugin.Section("SectionName", sectionAction));
		}

		[Fact]
		public void AddNewSection()
		{
			var innerSection = new TabSection();
			Plugin.Section("SectionName1", innerSection);

			Assert.Equal(1, Plugin.Rows.Count());
			Assert.Equal(2, Plugin.Rows.Single().Columns.Count());
			Assert.Equal("*SectionName1*", Plugin.Rows.Single().Columns.First().Data);

            var buildResult = innerSection.Build() as IEnumerable<object>;
            Assert.Equal(0, buildResult.Count());
		}

		[Fact]
		public void AddSectionWithThreeRows()
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

            var buildResult = rows as IEnumerable<object>;
            Assert.Equal(3, buildResult.Count());
		}

		private TabSection Plugin { get; set; }

		public PluginShould()
		{
			Plugin = Glimpse.Core.Plugin.Assist.Plugin.Create();
		}
	}
}