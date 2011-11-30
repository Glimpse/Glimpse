using System;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public static class GlimpseRowFormattingExtensions
	{
		public static GlimpseRow Bold(this GlimpseRow row)
		{
			return ApplyToLastColumn(row, Formats.Bold);
		}

		public static GlimpseRow BoldIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Bold() : row;
		}

		public static GlimpseRow Italic(this GlimpseRow row)
		{
			return ApplyToLastColumn(row, Formats.Italic);
		}

		public static GlimpseRow ItalicIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Italic() : row;
		}

		public static GlimpseRow Raw(this GlimpseRow row)
		{
			return ApplyToLastColumn(row, Formats.Raw);
		}

		public static GlimpseRow RawIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Raw() : row;
		}

		public static GlimpseRow Sub(this GlimpseRow row)
		{
			return ApplyToLastColumn(row, Formats.Sub);
		}

		public static GlimpseRow SubIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Sub() : row;
		}

		public static GlimpseRow Underline(this GlimpseRow row)
		{
			return ApplyToLastColumn(row, Formats.Underline);
		}

		public static GlimpseRow UnderlineIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Underline() : row;
		}


		public static GlimpseRow Error(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Error");
		}

		public static GlimpseRow ErrorIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Error() : row;
		}

		public static GlimpseRow Fail(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Fail");
		}

		public static GlimpseRow FailIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Fail() : row;
		}

		public static GlimpseRow Info(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Info");
		}

		public static GlimpseRow InfoIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Info() : row;
		}

		public static GlimpseRow Ms(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Ms");
		}

		public static GlimpseRow MsIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Ms() : row;
		}

		public static GlimpseRow Quiet(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Quiet");
		}

		public static GlimpseRow QuietIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Quiet() : row;
		}

		public static GlimpseRow Selected(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Selected");
		}

		public static GlimpseRow SelectedIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Selected() : row;
		}

		public static GlimpseRow Loading(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Loading");
		}

		public static GlimpseRow LoadingIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Loading() : row;
		}

		public static GlimpseRow Warn(this GlimpseRow row)
		{
			return VerifyAndApplyFormatting(row, "Warn");
		}

		public static GlimpseRow WarnIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Warn() : row;
		}


		private static GlimpseRow VerifyAndApplyFormatting(GlimpseRow row, string operation)
		{
			if (row.Columns.Count() <= 0)
				throw new InvalidOperationException(String.Format("The operation '{0}' is only valid when row has columns.", operation));

			row.Column(operation.ToLower());
			return row;
		}

		private static GlimpseRow ApplyToLastColumn(GlimpseRow row, string format)
		{
			var data = row.Columns.Last().Data;
			var formattedData = format.FormatWith(data);
			row.Columns.Last().OverrideData(formattedData);
			return row;
		}
	}
}