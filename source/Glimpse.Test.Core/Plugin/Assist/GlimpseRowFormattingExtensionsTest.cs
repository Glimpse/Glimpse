using System;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin.Assist
{
    [TestFixture]
	public class GlimpseRowFormattingExtensionsTest
    {
        [Test]
		public void GlimpseRow_Bold_AppliesBoldToLastColumn()
        {
            var row = Row.Bold();

			Assert.AreEqual(row.Columns.Last().Data, @"*Text*");
        }

		[Test]
		public void GlimpseRow_Italic_AppliesItalicToLastColumn()
		{
			var row = Row.Italic();

			Assert.AreEqual(row.Columns.Last().Data, @"\Text\");
		}

    	[Test]
		public void GlimpseRow_Raw_AppliesRawToLastColumn()
    	{
			var row = Row.Raw();

    		Assert.AreEqual(row.Columns.Last().Data, @"!Text!");
    	}

    	[Test]
		public void GlimpseRow_Sub_AppliesSubToLastColumn()
		{
			var row = Row.Sub();

			Assert.AreEqual(row.Columns.Last().Data, @"|Text|");
		}

    	[Test]
		public void GlimpseRow_Underline_AppliesUnderlineToLastColumn()
		{
			var row = Row.Underline();

			Assert.AreEqual(row.Columns.Last().Data, @"_Text_");
		}

		[Test]
		public void GlimpseRow_RowOperations_AreInvalidForRowsWithoutColumns()
		{
			var row = new GlimpseRow();

			Assert.Throws<InvalidOperationException>(() => row.Quiet());
		}

		[Test]
		public void GlimpseRow_Error_AddsColumnWithError()
		{
			var row = Row.Error();

			Assert.AreEqual(row.Columns.Last().Data, "error");
		}

		[Test]
		public void GlimpseRow_Fail_AddsColumnWithFail()
		{
			var row = Row.Fail();

			Assert.AreEqual(row.Columns.Last().Data, "fail");
		}

		[Test]
		public void GlimpseRow_Info_AddsColumnWithInfo()
		{
			var row = Row.Info();

			Assert.AreEqual(row.Columns.Last().Data, "info");
		}

		[Test]
		public void GlimpseRow_Loading_AddsColumnWithLoading()
		{
			var row = Row.Loading();

			Assert.AreEqual(row.Columns.Last().Data, "loading");
		}

		[Test]
		public void GlimpseRow_Ms_AddsColumnWithMs()
		{
			var row = Row.Ms();

			Assert.AreEqual(row.Columns.Last().Data, "ms");
		}

		[Test]
		public void GlimpseRow_Quiet_AddsColumnWithQuiet()
		{
			var row = Row.Quiet();

			Assert.AreEqual(row.Columns.Last().Data, "quiet");
		}

		[Test]
		public void GlimpseRow_Selected_AddsColumnWithSelected()
		{
			var row = Row.Selected();

			Assert.AreEqual(row.Columns.Last().Data, "selected");
		}

		[Test]
		public void GlimpseRow_Warn_AddsColumnWithWarn()
		{
			var row = Row.Warn();

			Assert.AreEqual(row.Columns.Last().Data, "warn");
		}
		
		private GlimpseRow Row { get; set; }

        [SetUp]
        public void Setup()
        {
			Row = new GlimpseRow().Column("Text");
        }
    }
}
