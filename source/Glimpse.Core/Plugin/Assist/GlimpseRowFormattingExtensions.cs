using System;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public static class GlimpseRowFormattingExtensions
	{
		public static GlimpseRow Bold(this GlimpseRow row)
		{
			ApplyToLastColumn(row, Formats.Bold);
			return row;
		}

		public static GlimpseRow BoldIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Bold() : row;
		}

		public static GlimpseRow Underline(this GlimpseRow row)
		{
			ApplyToLastColumn(row, Formats.Underline);
			return row;
		}

		public static GlimpseRow UnderlineIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Underline() : row;
		}


		public static GlimpseRow Quiet(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Quiet");
			row.Column("quiet");
			return row;
		}

		public static GlimpseRow QuietIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Quiet() : row;
		}

		public static GlimpseRow Selected(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Selected");
			row.Column("selected");
			return row;
		}

		public static GlimpseRow SelectedIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Selected() : row;
		}

		public static GlimpseRow Warn(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Warn");
			row.Column("warn");
			return row;
		}

		public static GlimpseRow WarnIf(this GlimpseRow row, bool condition)
		{
			return condition ? row.Warn() : row;
		}


		private static void VerifyRowOperation(GlimpseRow row, string operation)
		{
			if (row.Columns.Count() <= 0)
				throw new InvalidOperationException(String.Format("The operation '{0}' is only valid when row has columns.", operation));
		}

		private static void ApplyToLastColumn(GlimpseRow row, string format)
		{
			var data = row.Columns.Last().Data;
			var formattedData = format.FormatWith(data);
			row.Columns.Last().OverrideData(formattedData);
		}
	}
}