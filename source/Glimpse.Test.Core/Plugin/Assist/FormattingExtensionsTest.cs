using System;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class FormattingExtensionsFact
	{
		[Fact]
		public void GlimpseRow_Bold_AppliesBoldToLastColumn()
		{
			var row = Row.Bold();

			Assert.Equal(row.Columns.Last().Data, "*Text*");
		}

		[Fact]
		public void GlimpseRow_Underline_AppliesUnderlineToLastColumn()
		{
			var row = Row.Underline();

			Assert.Equal(row.Columns.Last().Data, "_Text_");
		}

		[Fact]
		public void GlimpseRow_RowOperation_IsInvalidForRowsWithoutColumns()
		{
			var row = new GlimpseRow();

			Assert.Throws<InvalidOperationException>(() => row.Quiet());
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

		public FormattingExtensionsFact()
		{
			Row = new GlimpseRow().Column("Text");
		}
	}
}
