using System;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class GlimpseRowFormattingExtensionsFact
	{
		[Fact]
		public void GlimpseRow_Bold_AppliesBoldToLastColumn()
		{
			var row = Row.Bold();

			Assert.Equal(row.Columns.Last().Data, @"*Text*");
		}

		[Fact]
		public void GlimpseRow_Italic_AppliesItalicToLastColumn()
		{
			var row = Row.Italic();

			Assert.Equal(row.Columns.Last().Data, @"\Text\");
		}

		[Fact]
		public void GlimpseRow_Raw_AppliesRawToLastColumn()
		{
			var row = Row.Raw();

			Assert.Equal(row.Columns.Last().Data, @"!Text!");
		}

		[Fact]
		public void GlimpseRow_Sub_AppliesSubToLastColumn()
		{
			var row = Row.Sub();

			Assert.Equal(row.Columns.Last().Data, @"|Text|");
		}

		[Fact]
		public void GlimpseRow_Underline_AppliesUnderlineToLastColumn()
		{
			var row = Row.Underline();

			Assert.Equal(row.Columns.Last().Data, "_Text_");
		}

		[Fact]
		public void GlimpseRow_RowOperations_AreInvalidForRowsWithoutColumns()
		{
			var row = new GlimpseRow();

			Assert.Throws<InvalidOperationException>(() => row.Quiet());
		}

		[Fact]
		public void GlimpseRow_Error_AddsColumnWithError()
		{
			var row = Row.Error();

			Assert.Equal(row.Columns.Last().Data, "error");
		}

		[Fact]
		public void GlimpseRow_Fail_AddsColumnWithFail()
		{
			var row = Row.Fail();

			Assert.Equal(row.Columns.Last().Data, "fail");
		}

		[Fact]
		public void GlimpseRow_Info_AddsColumnWithInfo()
		{
			var row = Row.Info();

			Assert.Equal(row.Columns.Last().Data, "info");
		}

		[Fact]
		public void GlimpseRow_Loading_AddsColumnWithLoading()
		{
			var row = Row.Loading();

			Assert.Equal(row.Columns.Last().Data, "loading");
		}

		[Fact]
		public void GlimpseRow_Ms_AddsColumnWithMs()
		{
			var row = Row.Ms();

			Assert.Equal(row.Columns.Last().Data, "ms");
		}

		[Fact]
		public void GlimpseRow_Quiet_AddsColumnWithQuiet()
		{
			var row = Row.Quiet();

			Assert.Equal(row.Columns.Last().Data, "quiet");
		}

		[Fact]
		public void GlimpseRow_Selected_AddsColumnWithSelected()
		{
			var row = Row.Selected();

			Assert.Equal(row.Columns.Last().Data, "selected");
		}

		[Fact]
		public void GlimpseRow_Warn_AddsColumnWithWarn()
		{
			var row = Row.Warn();

			Assert.Equal(row.Columns.Last().Data, "warn");
		}

		private GlimpseRow Row { get; set; }

		public GlimpseRowFormattingExtensionsFact()
		{
			Row = new GlimpseRow().Column("Text");
		}
	}
}
