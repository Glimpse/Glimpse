using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabRowFormattingExtensionsShould
	{
		[Fact]
		public void ApplyStrongToLastColumn()
		{
			SectionRow.Strong();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), @"*Text*");
		}

        [Fact]
        public void ApplyStrongToLastColumnIf()
        {
            SectionRow.StrongIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.StrongIf(true); 
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"*Text*");
        }

		[Fact]
		public void ApplyEmphasisToLastColumn()
		{
			SectionRow.Emphasis();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), @"\Text\");
		}

        [Fact]
        public void ApplyEmphasisToLastColumnIf()
        {
            SectionRow.EmphasisIf(false);
            var row = (IEnumerable<object>)SectionRow.Build(); 

            Assert.Equal(row.Last(), @"Text");

            SectionRow.EmphasisIf(true);
            row = (IEnumerable<object>)SectionRow.Build(); 

            Assert.Equal(row.Last(), @"\Text\");
        }

		[Fact]
		public void ApplyRawToLastColumn()
		{
            SectionRow.Raw();
            var row = (IEnumerable<object>)SectionRow.Build(); 

			Assert.Equal(row.Last(), @"!Text!");
		}

        [Fact]
        public void ApplyRawToLastColumnIf()
        {
            SectionRow.RawIf(false);
            var row = (IEnumerable<object>)SectionRow.Build(); 

            Assert.Equal(row.Last(), @"Text");

            SectionRow.RawIf(true);
            row = (IEnumerable<object>)SectionRow.Build(); 

            Assert.Equal(row.Last(), @"!Text!");
        }

		[Fact]
		public void ApplySubToLastColumn()
		{
			SectionRow.Sub();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), @"|Text|");
		}

        [Fact]
        public void ApplySubToLastColumnIf()
        {
            SectionRow.SubIf(false);
            var row = (IEnumerable<object>)SectionRow.Build(); 

            Assert.Equal(row.Last(), @"Text");

            SectionRow.SubIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"|Text|");
        }

		[Fact]
		public void ApplyUnderlineToLastColumn()
		{
			SectionRow.Underline();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "_Text_");
		}

        [Fact]
        public void ApplyUnderlineToLastColumnIf()
        {
            SectionRow.UnderlineIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.UnderlineIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"_Text_");
        }

		[Fact]
		public void AddColumnWithError()
		{
			SectionRow.Error();

            var row = (IEnumerable<object>)SectionRow.Build();
            Assert.Equal(row.Last(), "error");
		}

        [Fact]
        public void AddColumnWithErrorIf()
        {
            SectionRow.ErrorIf(false); 
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.ErrorIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"error");
        }

		[Fact]
		public void AddColumnWithFail()
		{
            SectionRow.Fail();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "fail");
		}

        [Fact]
        public void AddColumnWithFailIf()
        {
            SectionRow.FailIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.FailIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"fail");
        }

		[Fact]
		public void AddColumnWithInfo()
		{
            SectionRow.Info();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "info");
		}

        [Fact]
        public void AddColumnWithInfoIf()
        {
            SectionRow.InfoIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.InfoIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"info");
        }

		[Fact]
		public void AddColumnWithLoading()
		{
            SectionRow.Loading();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "loading");
		}

        [Fact]
        public void AddColumnWithLodaingIf()
        {
            SectionRow.LoadingIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.LoadingIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"loading");
        }

		[Fact]
		public void AddColumnWithMs()
		{
            SectionRow.Ms();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "ms");
		}

        [Fact]
        public void AddColumnWithMsIf()
        {
            SectionRow.MsIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.MsIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"ms");
        }

		[Fact]
		public void AddColumnWithQuiet()
		{
            SectionRow.Quiet();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "quiet");
		}

        [Fact]
        public void AddColumnWithQuietIf()
        {
            SectionRow.QuietIf(false);
            var row = (IEnumerable<object>)SectionRow.Build(); 

            Assert.Equal(row.Last(), @"Text");

            SectionRow.QuietIf(true);
            row = (IEnumerable<object>)SectionRow.Build(); 

            Assert.Equal(row.Last(), @"quiet");
        }

		[Fact]
		public void AddColumnWithSelected()
		{
            SectionRow.Selected();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "selected");
		}

        [Fact]
        public void AddColumnWithSelectedIf()
        {
            SectionRow.SelectedIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.SelectedIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"selected");
        }


		[Fact]
		public void AddColumnWithWarn()
		{
            SectionRow.Warn();
            var row = (IEnumerable<object>)SectionRow.Build();

			Assert.Equal(row.Last(), "warn");
		}

        [Fact]
        public void AddColumnWithWarnIf()
        {
            SectionRow.WarnIf(false);
            var row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"Text");

            SectionRow.WarnIf(true);
            row = (IEnumerable<object>)SectionRow.Build();

            Assert.Equal(row.Last(), @"warn");
        }

		private TabSectionRow SectionRow { get; set; }

		public TabRowFormattingExtensionsShould()
		{
			SectionRow = new TabSectionRow().Column("Text");
		}
	}
}
