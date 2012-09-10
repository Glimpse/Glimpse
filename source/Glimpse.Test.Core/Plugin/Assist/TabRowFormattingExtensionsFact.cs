using System;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabRowFormattingExtensionsFact
	{
		[Fact]
		public void TabRow_Strong_AppliesStrongToLastColumn()
		{
			var row = Row.Strong();

			Assert.Equal(row.Columns.Last().Data, @"*Text*");
		}

		[Fact]
		public void TabRow_Emphasis_AppliesEmphasisToLastColumn()
		{
			var row = Row.Emphasis();

			Assert.Equal(row.Columns.Last().Data, @"\Text\");
		}

		[Fact]
		public void TabRow_Raw_AppliesRawToLastColumn()
		{
			var row = Row.Raw();

			Assert.Equal(row.Columns.Last().Data, @"!Text!");
		}

		[Fact]
		public void TabRow_Sub_AppliesSubToLastColumn()
		{
			var row = Row.Sub();

			Assert.Equal(row.Columns.Last().Data, @"|Text|");
		}

		[Fact]
		public void TabRow_Underline_AppliesUnderlineToLastColumn()
		{
			var row = Row.Underline();

			Assert.Equal(row.Columns.Last().Data, "_Text_");
		}

		[Fact]
		public void TabRow_RowOperations_AreInvalidForRowsWithoutColumns()
		{
			var row = new TabRow();

			Assert.Throws<InvalidOperationException>(() => row.Quiet());
		}

		[Fact]
		public void TabRow_Error_AddsColumnWithError()
		{
			var row = Row.Error();

			Assert.Equal(row.Columns.Last().Data, "error");
		}

		[Fact]
		public void TabRow_Fail_AddsColumnWithFail()
		{
			var row = Row.Fail();

			Assert.Equal(row.Columns.Last().Data, "fail");
		}

		[Fact]
		public void TabRow_Info_AddsColumnWithInfo()
		{
			var row = Row.Info();

			Assert.Equal(row.Columns.Last().Data, "info");
		}

		[Fact]
		public void TabRow_Loading_AddsColumnWithLoading()
		{
			var row = Row.Loading();

			Assert.Equal(row.Columns.Last().Data, "loading");
		}

		[Fact]
		public void TabRow_Ms_AddsColumnWithMs()
		{
			var row = Row.Ms();

			Assert.Equal(row.Columns.Last().Data, "ms");
		}

		[Fact]
		public void TabRow_Quiet_AddsColumnWithQuiet()
		{
			var row = Row.Quiet();

			Assert.Equal(row.Columns.Last().Data, "quiet");
		}

		[Fact]
		public void TabRow_Selected_AddsColumnWithSelected()
		{
			var row = Row.Selected();

			Assert.Equal(row.Columns.Last().Data, "selected");
		}

		[Fact]
		public void TabRow_Warn_AddsColumnWithWarn()
		{
			var row = Row.Warn();

			Assert.Equal(row.Columns.Last().Data, "warn");
		}

		private TabRow Row { get; set; }

		public TabRowFormattingExtensionsFact()
		{
			Row = new TabRow().Column("Text");
		}
	}
}
