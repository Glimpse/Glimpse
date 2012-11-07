using System;
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
			var row = SectionRow.Strong();

			Assert.Equal(row.Columns.Last().Data, @"*Text*");
		}

        [Fact]
        public void ApplyStrongToLastColumnIf()
        {
            var row = SectionRow.StrongIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.StrongIf(true);

            Assert.Equal(row.Columns.Last().Data, @"*Text*");
        }

		[Fact]
		public void ApplyEmphasisToLastColumn()
		{
			var row = SectionRow.Emphasis();

			Assert.Equal(row.Columns.Last().Data, @"\Text\");
		}

        [Fact]
        public void ApplyEmphasisToLastColumnIf()
        {
            var row = SectionRow.EmphasisIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.EmphasisIf(true);

            Assert.Equal(row.Columns.Last().Data, @"\Text\");
        }

		[Fact]
		public void ApplyRawToLastColumn()
		{
			var row = SectionRow.Raw();

			Assert.Equal(row.Columns.Last().Data, @"!Text!");
		}

        [Fact]
        public void ApplyRawToLastColumnIf()
        {
            var row = SectionRow.RawIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.RawIf(true);

            Assert.Equal(row.Columns.Last().Data, @"!Text!");
        }

		[Fact]
		public void ApplySubToLastColumn()
		{
			var row = SectionRow.Sub();

			Assert.Equal(row.Columns.Last().Data, @"|Text|");
		}

        [Fact]
        public void ApplySubToLastColumnIf()
        {
            var row = SectionRow.SubIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.SubIf(true);

            Assert.Equal(row.Columns.Last().Data, @"|Text|");
        }

		[Fact]
		public void ApplyUnderlineToLastColumn()
		{
			var row = SectionRow.Underline();

			Assert.Equal(row.Columns.Last().Data, "_Text_");
		}

        [Fact]
        public void ApplyUnderlineToLastColumnIf()
        {
            var row = SectionRow.UnderlineIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.UnderlineIf(true);

            Assert.Equal(row.Columns.Last().Data, @"_Text_");
        }

		[Fact]
		public void ThrowForRowOperationsWithoutColumns()
		{
			var row = new TabSectionRow();

			Assert.Throws<InvalidOperationException>(() => row.Quiet());
		}

		[Fact]
		public void AddColumnWithError()
		{
			var row = SectionRow.Error();

			Assert.Equal(row.Columns.Last().Data, "error");
		}

        [Fact]
        public void AddColumnWithErrorIf()
        {
            var row = SectionRow.ErrorIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.ErrorIf(true);

            Assert.Equal(row.Columns.Last().Data, @"error");
        }

		[Fact]
		public void AddColumnWithFail()
		{
			var row = SectionRow.Fail();

			Assert.Equal(row.Columns.Last().Data, "fail");
		}

        [Fact]
        public void AddColumnWithFailIf()
        {
            var row = SectionRow.FailIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.FailIf(true);

            Assert.Equal(row.Columns.Last().Data, @"fail");
        }

		[Fact]
		public void AddColumnWithInfo()
		{
			var row = SectionRow.Info();

			Assert.Equal(row.Columns.Last().Data, "info");
		}

        [Fact]
        public void AddColumnWithInfoIf()
        {
            var row = SectionRow.InfoIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.InfoIf(true);

            Assert.Equal(row.Columns.Last().Data, @"info");
        }

		[Fact]
		public void AddColumnWithLoading()
		{
			var row = SectionRow.Loading();

			Assert.Equal(row.Columns.Last().Data, "loading");
		}

        [Fact]
        public void AddColumnWithLodaingIf()
        {
            var row = SectionRow.LoadingIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.LoadingIf(true);

            Assert.Equal(row.Columns.Last().Data, @"loading");
        }

		[Fact]
		public void AddColumnWithMs()
		{
			var row = SectionRow.Ms();

			Assert.Equal(row.Columns.Last().Data, "ms");
		}

        [Fact]
        public void AddColumnWithMsIf()
        {
            var row = SectionRow.MsIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.MsIf(true);

            Assert.Equal(row.Columns.Last().Data, @"ms");
        }

		[Fact]
		public void AddColumnWithQuiet()
		{
			var row = SectionRow.Quiet();

			Assert.Equal(row.Columns.Last().Data, "quiet");
		}

        [Fact]
        public void AddColumnWithQuietIf()
        {
            var row = SectionRow.QuietIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.QuietIf(true);

            Assert.Equal(row.Columns.Last().Data, @"quiet");
        }

		[Fact]
		public void AddColumnWithSelected()
		{
			var row = SectionRow.Selected();

			Assert.Equal(row.Columns.Last().Data, "selected");
		}

        [Fact]
        public void AddColumnWithSelectedIf()
        {
            var row = SectionRow.SelectedIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.SelectedIf(true);

            Assert.Equal(row.Columns.Last().Data, @"selected");
        }


		[Fact]
		public void AddColumnWithWarn()
		{
			var row = SectionRow.Warn();

			Assert.Equal(row.Columns.Last().Data, "warn");
		}

        [Fact]
        public void AddColumnWithWarnIf()
        {
            var row = SectionRow.WarnIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = SectionRow.WarnIf(true);

            Assert.Equal(row.Columns.Last().Data, @"warn");
        }

		private TabSectionRow SectionRow { get; set; }

		public TabRowFormattingExtensionsShould()
		{
			SectionRow = new TabSectionRow().Column("Text");
		}
	}
}
