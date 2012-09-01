using System;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public static class FormattingExtensions
	{
		public static GlimpseRow Bold(this GlimpseRow row)
		{
			ApplyToLastColumn(row, "*{0}*");
			return row;
		}

		public static GlimpseRow Underline(this GlimpseRow row)
		{
			ApplyToLastColumn(row, "_{0}_");
			return row;
		}


		public static GlimpseRow Quiet(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Quiet");
			row.Column("quiet");
			return row;
		}
		
		public static GlimpseRow Selected(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Selected");
			row.Column("selected");
			return row;
		}

		public static GlimpseRow Warn(this GlimpseRow row)
		{
			VerifyRowOperation(row, "Warn");
			row.Column("warn");
			return row;
		}


		private static void VerifyRowOperation(GlimpseRow row, string operation)
		{
			if (row.Columns.Count() <= 0)
				throw new InvalidOperationException(String.Format("The operation '{0}' is only valid when row has columns.", operation));
		}

		private static void ApplyToLastColumn(GlimpseRow row, string format)
		{
			var data = row.Columns.Last().Data;
			var formattedData = String.Format(format, data);
			row.Columns.Last().OverrideData(formattedData);
		}
	}
}