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
			var row = Row.Strong();

			Assert.Equal(row.Columns.Last().Data, @"*Text*");
		}

        [Fact]
        public void ApplyStrongToLastColumnIf()
        {
            var row = Row.StrongIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.StrongIf(true);

            Assert.Equal(row.Columns.Last().Data, @"*Text*");
        }

		[Fact]
		public void ApplyEmphasisToLastColumn()
		{
			var row = Row.Emphasis();

			Assert.Equal(row.Columns.Last().Data, @"\Text\");
		}

        [Fact]
        public void ApplyEmphasisToLastColumnIf()
        {
            var row = Row.EmphasisIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.EmphasisIf(true);

            Assert.Equal(row.Columns.Last().Data, @"\Text\");
        }

		[Fact]
		public void ApplyRawToLastColumn()
		{
			var row = Row.Raw();

			Assert.Equal(row.Columns.Last().Data, @"!Text!");
		}

        [Fact]
        public void ApplyRawToLastColumnIf()
        {
            var row = Row.RawIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.RawIf(true);

            Assert.Equal(row.Columns.Last().Data, @"!Text!");
        }

		[Fact]
		public void ApplySubToLastColumn()
		{
			var row = Row.Sub();

			Assert.Equal(row.Columns.Last().Data, @"|Text|");
		}

        [Fact]
        public void ApplySubToLastColumnIf()
        {
            var row = Row.SubIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.SubIf(true);

            Assert.Equal(row.Columns.Last().Data, @"|Text|");
        }

		[Fact]
		public void ApplyUnderlineToLastColumn()
		{
			var row = Row.Underline();

			Assert.Equal(row.Columns.Last().Data, "_Text_");
		}

        [Fact]
        public void ApplyUnderlineToLastColumnIf()
        {
            var row = Row.UnderlineIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.UnderlineIf(true);

            Assert.Equal(row.Columns.Last().Data, @"_Text_");
        }

		[Fact]
		public void ThrowForRowOperationsWithoutColumns()
		{
			var row = new TabRow();

			Assert.Throws<InvalidOperationException>(() => row.Quiet());
		}

		[Fact]
		public void AddColumnWithError()
		{
			var row = Row.Error();

			Assert.Equal(row.Columns.Last().Data, "error");
		}

        [Fact]
        public void AddColumnWithErrorIf()
        {
            var row = Row.ErrorIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.ErrorIf(true);

            Assert.Equal(row.Columns.Last().Data, @"error");
        }

		[Fact]
		public void AddColumnWithFail()
		{
			var row = Row.Fail();

			Assert.Equal(row.Columns.Last().Data, "fail");
		}

        [Fact]
        public void AddColumnWithFailIf()
        {
            var row = Row.FailIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.FailIf(true);

            Assert.Equal(row.Columns.Last().Data, @"fail");
        }

		[Fact]
		public void AddColumnWithInfo()
		{
			var row = Row.Info();

			Assert.Equal(row.Columns.Last().Data, "info");
		}

        [Fact]
        public void AddColumnWithInfoIf()
        {
            var row = Row.InfoIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.InfoIf(true);

            Assert.Equal(row.Columns.Last().Data, @"info");
        }

		[Fact]
		public void AddColumnWithLoading()
		{
			var row = Row.Loading();

			Assert.Equal(row.Columns.Last().Data, "loading");
		}

        [Fact]
        public void AddColumnWithLodaingIf()
        {
            var row = Row.LoadingIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.LoadingIf(true);

            Assert.Equal(row.Columns.Last().Data, @"loading");
        }

		[Fact]
		public void AddColumnWithMs()
		{
			var row = Row.Ms();

			Assert.Equal(row.Columns.Last().Data, "ms");
		}

        [Fact]
        public void AddColumnWithMsIf()
        {
            var row = Row.MsIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.MsIf(true);

            Assert.Equal(row.Columns.Last().Data, @"ms");
        }

		[Fact]
		public void AddColumnWithQuiet()
		{
			var row = Row.Quiet();

			Assert.Equal(row.Columns.Last().Data, "quiet");
		}

        [Fact]
        public void AddColumnWithQuietIf()
        {
            var row = Row.QuietIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.QuietIf(true);

            Assert.Equal(row.Columns.Last().Data, @"quiet");
        }

		[Fact]
		public void AddColumnWithSelected()
		{
			var row = Row.Selected();

			Assert.Equal(row.Columns.Last().Data, "selected");
		}

        [Fact]
        public void AddColumnWithSelectedIf()
        {
            var row = Row.SelectedIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.SelectedIf(true);

            Assert.Equal(row.Columns.Last().Data, @"selected");
        }


		[Fact]
		public void AddColumnWithWarn()
		{
			var row = Row.Warn();

			Assert.Equal(row.Columns.Last().Data, "warn");
		}

        [Fact]
        public void AddColumnWithWarnIf()
        {
            var row = Row.WarnIf(false);

            Assert.Equal(row.Columns.Last().Data, @"Text");

            row = Row.WarnIf(true);

            Assert.Equal(row.Columns.Last().Data, @"warn");
        }

		private TabRow Row { get; set; }

		public TabRowFormattingExtensionsShould()
		{
			Row = new TabRow().Column("Text");
		}
	}
}
