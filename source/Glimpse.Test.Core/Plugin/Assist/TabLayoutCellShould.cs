using System;
using System.Collections.Generic;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabLayoutCellShould
	{
		[Fact]
		public void SetDataId()
		{
			var cell = new TabLayoutCell(1);
			
			Assert.Equal(1, cell.Data);
		}

		[Fact]
		public void ThrowForNegativeValue()
		{
			Assert.Throws<ArgumentException>(() => new TabLayoutCell(-1));
		}

		[Fact]
		public void SetDataFormatOnConstruction()
		{
			var cell = new TabLayoutCell("{0} - {1}");

			Assert.Equal("{0} - {1}", cell.Data);
		}

		[Fact]
		public void ThrowForNullOrEmptyConstruction()
		{
			Assert.Throws<ArgumentException>(() => new TabLayoutCell(null));
			Assert.Throws<ArgumentException>(() => new TabLayoutCell(""));
		}

		[Fact]
		public void ThrowWhenNullOrEmpty()
		{
		    Assert.Throws<ArgumentException>(() => Cell.Format(""));
		    Assert.Throws<ArgumentException>(() => Cell.Format(null));
		}

		[Fact]
		public void SetDataFormat()
		{
			Cell.Format("{0} -> {1} <- {2}");

			Assert.Equal("{0} -> {1} <- {2}", Cell.Data);
		}

		[Fact]
		public void SetStructureToRows()
		{
			var layout = TabLayout.Create();
			layout.Row(r => { }).Row(r => {});

			Cell.SetLayout(layout);

			Assert.NotEqual(layout.Rows, Cell.Layout);
		}

		[Fact]
		public void AddRowsToStructure()
		{
			IEnumerable<TabLayoutRow> rows = null;

			Cell.SetLayout(layout =>
			{
				layout.Row(r => { }).Row(r => { });

				rows = layout.Rows;
			});

			Assert.Equal(rows, Cell.Layout);
		}

		[Fact]
		public void SetIsKey()
		{
			Cell.AsKey();

			Assert.Equal(true, Cell.Key);
		}

		[Fact]
		public void SetIsCodeAndCodeType()
		{
			Cell.AsCode(CodeType.Sql);

			Assert.Equal(true, Cell.IsCode);
			Assert.Equal(CodeType.Sql.ToString().ToLower(), Cell.CodeType);
		}

		[Fact]
		public void SetAlign()
		{
			Cell.AlignRight();

			Assert.Equal("Right", Cell.Align);
		}

		[Fact]
		public void ThrowForNegativeValuePixelValue()
		{
			Assert.Throws<ArgumentException>(() => Cell.WidthInPixels(-1));
		}

		[Fact]
		public void SetWidthInPixels()
		{
			Cell.WidthInPixels(123);

			Assert.Equal("123px", Cell.Width);
		}

		[Fact]
		public void ThrowForNegativePercentValue()
		{
			Assert.Throws<ArgumentException>(() => Cell.WidthInPercent(-1));
		}

		[Fact]
		public void SetWidthInPecent()
		{
			Cell.WidthInPercent(123);

			Assert.Equal("123%", Cell.Width);
		}

		[Fact]
		public void ThrowForValueLessThenOne()
		{
			Assert.Throws<ArgumentException>(() => Cell.SpanRows(-1));
			Assert.Throws<ArgumentException>(() => Cell.SpanRows(0));
		}

		[Fact]
		public void SetRowSpan()
		{
			Cell.SpanRows(3);

			Assert.Equal(3, Cell.RowSpan);
		}

		[Fact]
		public void ThrowForNullOrEmptyClass()
		{
			Assert.Throws<ArgumentException>(() => Cell.Class(null));
			Assert.Throws<ArgumentException>(() => Cell.Class(""));
		}

		[Fact]
		public void SetClassName()
		{
			Cell.Class("mono");

			Assert.Equal("mono", Cell.ClassName);
		}

		[Fact]
		public void SetSuppressAutoPreview()
		{
			Cell.DisableLimit();

			Assert.Equal(true, Cell.SuppressAutoPreview);
		}

		[Fact]
		public void ThrowForValueLessThenOneLimitTo()
		{
			Assert.Throws<ArgumentException>(() => Cell.LimitTo(-1));
			Assert.Throws<ArgumentException>(() => Cell.LimitTo(0));
		}

		[Fact]
		public void SetLimit()
		{
			Cell.LimitTo(10);

			Assert.Equal(10, Cell.Limit);
		}

		[Fact]
		public void ThrowForNullOrEmptyPrefix()
		{
			Assert.Throws<ArgumentException>(() => Cell.Prefix(null));
			Assert.Throws<ArgumentException>(() => Cell.Prefix(""));
		}

		[Fact]
		public void SetPre()
		{
			Cell.Prefix("T+ ");

			Assert.Equal("T+ ", Cell.Pre);
		}

		[Fact]
		public void ThrowForNullOrEmptySuffix()
		{
			Assert.Throws<ArgumentException>(() => Cell.Suffix(null));
			Assert.Throws<ArgumentException>(() => Cell.Suffix(""));
		}

		[Fact]
		public void SetPost()
		{
			Cell.Suffix(" ms");

			Assert.Equal(" ms", Cell.Post);
		}

		private TabLayoutCell Cell { get; set; }

		public TabLayoutCellShould()
		{
			Cell = new TabLayoutCell(1);
		}
	}
}