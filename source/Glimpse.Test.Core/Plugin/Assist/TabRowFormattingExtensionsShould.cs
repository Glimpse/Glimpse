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
		public void ApplyEmphasisToLastColumn()
		{
			var row = Row.Emphasis();

			Assert.Equal(row.Columns.Last().Data, @"\Text\");
		}

		[Fact]
		public void ApplyRawToLastColumn()
		{
			var row = Row.Raw();

			Assert.Equal(row.Columns.Last().Data, @"!Text!");
		}

		[Fact]
		public void ApplySubToLastColumn()
		{
			var row = Row.Sub();

			Assert.Equal(row.Columns.Last().Data, @"|Text|");
		}

		[Fact]
		public void ApplyUnderlineToLastColumn()
		{
			var row = Row.Underline();

			Assert.Equal(row.Columns.Last().Data, "_Text_");
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
		public void AddColumnWithFail()
		{
			var row = Row.Fail();

			Assert.Equal(row.Columns.Last().Data, "fail");
		}

		[Fact]
		public void AddColumnWithInfo()
		{
			var row = Row.Info();

			Assert.Equal(row.Columns.Last().Data, "info");
		}

		[Fact]
		public void AddColumnWithLoading()
		{
			var row = Row.Loading();

			Assert.Equal(row.Columns.Last().Data, "loading");
		}

		[Fact]
		public void AddColumnWithMs()
		{
			var row = Row.Ms();

			Assert.Equal(row.Columns.Last().Data, "ms");
		}

		[Fact]
		public void AddColumnWithQuiet()
		{
			var row = Row.Quiet();

			Assert.Equal(row.Columns.Last().Data, "quiet");
		}

		[Fact]
		public void AddColumnWithSelected()
		{
			var row = Row.Selected();

			Assert.Equal(row.Columns.Last().Data, "selected");
		}

		[Fact]
		public void AddColumnWithWarn()
		{
			var row = Row.Warn();

			Assert.Equal(row.Columns.Last().Data, "warn");
		}

		private TabRow Row { get; set; }

		public TabRowFormattingExtensionsShould()
		{
			Row = new TabRow().Column("Text");
		}
	}
}
