using System;

namespace Glimpse.Core.Plugin.Assist
{
	public class StructuredLayoutCell
	{
		public StructuredLayoutCell(int cell)
		{
			if (cell < 0)
				throw new ArgumentException("Cell must not be a negative value.", "cell");

			Data = cell;
		}

		public StructuredLayoutCell(string format)
		{
			if (String.IsNullOrEmpty(format)) 
				throw new ArgumentException("Format must not be null or empty.", "format");

			Data = format;
		}

		public object Data { get; private set; }
		public object Structure { get; private set; }
		
		public bool? IsKey { get; private set; }
		public bool? IsCode { get; private set; }
		public string CodeType { get; private set; }
		
		public string Align { get; private set; }
		public string Width { get; private set; }
		public int? RowSpan { get; private set; }
		public string ClassName { get; private set; }

		public bool? SuppressAutoPreview { get; private set; }
		public int? Limit { get; private set; }

		public string Pre { get; private set; }
		public string Post { get; private set; }

		public StructuredLayoutCell Format(string format)
		{
			if (String.IsNullOrEmpty(format))
				throw new ArgumentException("Format must not be null or empty.", "format");

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
			IsKey = true;
			return this;
		}

		public StructuredLayoutCell AsCode(CodeType codeType)
		{
			IsCode = true;
			CodeType = CodeTypeConverter.Convert(codeType);
			return this;
		}

		public StructuredLayoutCell AlignRight()
		{
			Align = "Right";
			return this;
		}

		public StructuredLayoutCell WidthInPixels(int pixels)
		{
			if (pixels < 0)
				throw new ArgumentException("Pixels must not be a negative value.", "pixels");

			Width = pixels + "px";
			return this;
		}

		public StructuredLayoutCell WidthInPercent(int percent)
		{
			if (percent < 0)
				throw new ArgumentException("Percent must not be a negative value.", "percent");

			Width = percent + "%";
			return this;
		}

		public StructuredLayoutCell SpanRows(int rows)
		{
			if (rows < 1)
				throw new ArgumentException("Rows must not be less then 0.", "rows");

			RowSpan = rows;
			return this;
		}

		public StructuredLayoutCell Class(string className)
		{
			if (String.IsNullOrEmpty(className))
				throw new ArgumentException("Class name must not be null or empty.", "className");

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
			if (rows < 1)
				throw new ArgumentException("Rows must not be less then 0.", "rows");

			Limit = rows;
			return this;
		}

		public StructuredLayoutCell Prefix(string prefix)
		{
			if (String.IsNullOrEmpty(prefix))
				throw new ArgumentException("Prefix must not be null or empty.", "prefix");

			Pre = prefix;
			return this;
		}

		public StructuredLayoutCell Suffix(string suffix)
		{
			if (String.IsNullOrEmpty(suffix))
				throw new ArgumentException("Suffix must not be null or empty.", "suffix");

			Post = suffix;
			return this;
		}
	}
}