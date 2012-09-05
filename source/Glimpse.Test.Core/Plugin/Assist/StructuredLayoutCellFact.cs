using System;
using System.Collections.Generic;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StructuredLayoutCellFact
	{
		[Fact]
		public void StructuredLayoutCell_New_SetsDataId()
		{
			var cell = new StructuredLayoutCell(1);
			
			Assert.Equal(1, cell.Data);
		}

		[Fact]
		public void StructuredLayoutCell_New_ThrowsForNegativeValue()
		{
			Assert.Throws<ArgumentException>(() => new StructuredLayoutCell(-1));
		}

		[Fact]
		public void StructuredLayoutCell_New_SetsDataFormat()
		{
			var cell = new StructuredLayoutCell("{0} - {1}");

			Assert.Equal("{0} - {1}", cell.Data);
		}

		[Fact]
		public void StructuredLayoutCell_New_ThrowsForNullOrEmpty()
		{
			Assert.Throws<ArgumentException>(() => new StructuredLayoutCell(null));
			Assert.Throws<ArgumentException>(() => new StructuredLayoutCell(""));
		}

		[Fact]
		public void StructuredLayoutCell_Format_ThrowsWhenNullOrEmpty()
		{
			Assert.Throws<ArgumentException>(() => Cell.Format(null));
			Assert.Throws<ArgumentException>(() => Cell.Format(""));
		}

		[Fact]
		public void StructuredLayoutCell_Format_SetsDataFormat()
		{
			Cell.Format("{0} -> {1} <- {2}");

			Assert.Equal("{0} -> {1} <- {2}", Cell.Data);
		}

		[Fact]
		public void StructuredLayoutCell_Layout_SetsStructureToRows()
		{
			var structuredLayout = StructuredLayout.Create();
			structuredLayout.Row(r => { }).Row(r => {});

			Cell.Layout(structuredLayout);

			Assert.Equal(structuredLayout.Rows, Cell.Structure);
		}

		[Fact]
		public void StructuredLayoutCell_Layout_AddsRowsToStructure()
		{
			IEnumerable<StructuredLayoutRow> rows = null;

			Cell.Layout(structuredLayout =>
			{
				structuredLayout.Row(r => { }).Row(r => { });

				rows = structuredLayout.Rows;
			});

			Assert.Equal(rows, Cell.Structure);
		}

		[Fact]
		public void StructuredLayoutCell_AsKey_SetsIsKey()
		{
			Cell.AsKey();

			Assert.Equal(true, Cell.IsKey);
		}

		[Fact]
		public void StructuredLayoutCell_AsCode_SetsIsCodeAndCodeType()
		{
			Cell.AsCode(CodeType.Sql);

			Assert.Equal(true, Cell.IsCode);
			Assert.Equal(CodeType.Sql.ToString().ToLower(), Cell.CodeType);
		}

		[Fact]
		public void StructuredLayoutCell_AlignRight_SetsAlign()
		{
			Cell.AlignRight();

			Assert.Equal("Right", Cell.Align);
		}

		[Fact]
		public void StructuredLayoutCell_WidthInPixels_ThrowsForNegativeValue()
		{
			Assert.Throws<ArgumentException>(() => Cell.WidthInPixels(-1));
		}

		[Fact]
		public void StructuredLayoutCell_WidthInPixels_SetsWidth()
		{
			Cell.WidthInPixels(123);

			Assert.Equal("123px", Cell.Width);
		}

		[Fact]
		public void StructuredLayoutCell_WidthInPercent_ThrowsForNegativeValue()
		{
			Assert.Throws<ArgumentException>(() => Cell.WidthInPercent(-1));
		}

		[Fact]
		public void StructuredLayoutCell_WidthInPercent_SetsWidth()
		{
			Cell.WidthInPercent(123);

			Assert.Equal("123%", Cell.Width);
		}

		[Fact]
		public void StructuredLayoutCell_SpanRows_ThrowsForValueLessThenOne()
		{
			Assert.Throws<ArgumentException>(() => Cell.SpanRows(-1));
			Assert.Throws<ArgumentException>(() => Cell.SpanRows(0));
		}

		[Fact]
		public void StructuredLayoutCell_SpanRows_SetsRowSpan()
		{
			Cell.SpanRows(3);

			Assert.Equal(3, Cell.RowSpan);
		}

		[Fact]
		public void StructuredLayoutCell_Class_ThrowsForNullOrEmpty()
		{
			Assert.Throws<ArgumentException>(() => Cell.Class(null));
			Assert.Throws<ArgumentException>(() => Cell.Class(""));
		}

		[Fact]
		public void StructuredLayoutCell_Class_SetsClassName()
		{
			Cell.Class("mono");

			Assert.Equal("mono", Cell.ClassName);
		}

		[Fact]
		public void StructuredLayoutCell_DisableLimit_SetsSuppressAutoPreview()
		{
			Cell.DisableLimit();

			Assert.Equal(true, Cell.SuppressAutoPreview);
		}

		[Fact]
		public void StructuredLayoutCell_LimitTo_ThrowsForValueLessThenOne()
		{
			Assert.Throws<ArgumentException>(() => Cell.LimitTo(-1));
			Assert.Throws<ArgumentException>(() => Cell.LimitTo(0));
		}

		[Fact]
		public void StructuredLayoutCell_LimitTo_SetsLimit()
		{
			Cell.LimitTo(10);

			Assert.Equal(10, Cell.Limit);
		}

		[Fact]
		public void StructuredLayoutCell_Prefix_ThrowsForNullOrEmpty()
		{
			Assert.Throws<ArgumentException>(() => Cell.Prefix(null));
			Assert.Throws<ArgumentException>(() => Cell.Prefix(""));
		}

		[Fact]
		public void StructuredLayoutCell_Prefix_SetsPre()
		{
			Cell.Prefix("T+ ");

			Assert.Equal("T+ ", Cell.Pre);
		}

		[Fact]
		public void StructuredLayoutCell_Suffix_ThrowsForNullOrEmpty()
		{
			Assert.Throws<ArgumentException>(() => Cell.Suffix(null));
			Assert.Throws<ArgumentException>(() => Cell.Suffix(""));
		}

		[Fact]
		public void StructuredLayoutCell_Suffix_SetsPost()
		{
			Cell.Suffix(" ms");

			Assert.Equal(" ms", Cell.Post);
		}

		private StructuredLayoutCell Cell { get; set; }

		public StructuredLayoutCellFact()
		{
			Cell = new StructuredLayoutCell(1);
		}
	}
}