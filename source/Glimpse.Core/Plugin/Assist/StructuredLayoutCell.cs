using System;

namespace Glimpse.Core.Plugin.Assist
{
	public class StructuredLayoutCell
	{
		public StructuredLayoutCell(int cell)
		{
			Data = cell;
		}

		public StructuredLayoutCell(string format)
		{
			Data = format;
		}

		public object Data { get; private set; }
		public object Structure { get; set; }
		
		public bool? Key { get; private set; }
		public bool? IsCode { get; private set; }
		public string CodeType { get; private set; }
		
		public string Align { get; private set; }
		public string Width { get; private set; }
		protected int? Rspan { get; private set; }
		public string ClassName { get; private set; }

		public bool? SuppressAutoPreview { get; private set; }
		public int? Limit { get; private set; }

		public string Pre { get; private set; }
		public string Post { get; private set; }

		public StructuredLayoutCell Format(string format)
		{
			Data = format;
			return this;
		}

		public StructuredLayoutCell Layout(StructuredLayout layout)
		{
			Structure = layout.Rows;
			return this;
		}

		public StructuredLayoutCell Layout(Action<StructuredLayout> layout)
		{
			var structuredLayout = StructuredLayout.Create(layout);
			Structure = structuredLayout.Rows;
			return this;
		}

		public StructuredLayoutCell AsKey()
		{
			Key = true;
			return this;
		}

		public StructuredLayoutCell AsCode(string codeType)
		{
			IsCode = true;
			CodeType = codeType;
			return this;
		}

		public StructuredLayoutCell AlignRight()
		{
			Align = "Right";
			return this;
		}

		public StructuredLayoutCell WidthInPercent(int value)
		{
			Width = value + "%";
			return this;
		}

		public StructuredLayoutCell WidthInPixels(int value)
		{
			Width = value + "px";
			return this;
		}

		public StructuredLayoutCell RowSpan(int value)
		{
			Rspan = value;
			return this;
		}

		public StructuredLayoutCell Class(string className)
		{
			ClassName = className;
			return this;
		}

		public StructuredLayoutCell DisableLimit()
		{
			SuppressAutoPreview = true;
			return this;
		}

		public StructuredLayoutCell LimitTo(int rows)
		{
			Limit = rows;
			return this;
		}

		public StructuredLayoutCell Prefix(string text)
		{
			Pre = text;
			return this;
		}

		public StructuredLayoutCell Suffix(string text)
		{
			Post = text;
			return this;
		}
	}
}