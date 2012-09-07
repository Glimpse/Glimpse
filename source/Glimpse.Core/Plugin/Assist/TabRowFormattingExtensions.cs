using System;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public static class TabRowFormattingExtensions
	{
		public static TabRow Strong(this TabRow row)
		{
			return ApplyToLastColumn(row, Formats.Strong);
		}

		public static TabRow StrongIf(this TabRow row, bool condition)
		{
			return condition ? row.Strong() : row;
		}

		public static TabRow Italic(this TabRow row)
		{
			return ApplyToLastColumn(row, Formats.Italic);
		}

		public static TabRow ItalicIf(this TabRow row, bool condition)
		{
			return condition ? row.Italic() : row;
		}

		public static TabRow Raw(this TabRow row)
		{
			return ApplyToLastColumn(row, Formats.Raw);
		}

		public static TabRow RawIf(this TabRow row, bool condition)
		{
			return condition ? row.Raw() : row;
		}

		public static TabRow Sub(this TabRow row)
		{
			return ApplyToLastColumn(row, Formats.Sub);
		}

		public static TabRow SubIf(this TabRow row, bool condition)
		{
			return condition ? row.Sub() : row;
		}

		public static TabRow Underline(this TabRow row)
		{
			return ApplyToLastColumn(row, Formats.Underline);
		}

		public static TabRow UnderlineIf(this TabRow row, bool condition)
		{
			return condition ? row.Underline() : row;
		}


		public static TabRow Error(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Error);
		}

		public static TabRow ErrorIf(this TabRow row, bool condition)
		{
			return condition ? row.Error() : row;
		}

		public static TabRow Fail(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Fail);
		}

		public static TabRow FailIf(this TabRow row, bool condition)
		{
			return condition ? row.Fail() : row;
		}

		public static TabRow Info(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Info);
		}

		public static TabRow InfoIf(this TabRow row, bool condition)
		{
			return condition ? row.Info() : row;
		}

		public static TabRow Loading(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Loading);
		}

		public static TabRow LoadingIf(this TabRow row, bool condition)
		{
			return condition ? row.Loading() : row;
		}

		public static TabRow Ms(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Ms);
		}

		public static TabRow MsIf(this TabRow row, bool condition)
		{
			return condition ? row.Ms() : row;
		}

		public static TabRow Quiet(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Quiet);
		}

		public static TabRow QuietIf(this TabRow row, bool condition)
		{
			return condition ? row.Quiet() : row;
		}

		public static TabRow Selected(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Selected);
		}

		public static TabRow SelectedIf(this TabRow row, bool condition)
		{
			return condition ? row.Selected() : row;
		}
		
		public static TabRow Warn(this TabRow row)
		{
			return VerifyAndApplyFormatting(row, FormattingKeywords.Warn);
		}

		public static TabRow WarnIf(this TabRow row, bool condition)
		{
			return condition ? row.Warn() : row;
		}


		private static TabRow VerifyAndApplyFormatting(TabRow row, string operation)
		{
			if (!row.Columns.Any())
				throw new InvalidOperationException(String.Format("The operation '{0}' is only valid when row has columns.", operation));

			row.Column(operation.ToLower());
			return row;
		}

		private static TabRow ApplyToLastColumn(TabRow row, string format)
		{
			var data = row.Columns.Last().Data;
			var formattedData = format.FormatWith(data);
			row.Columns.Last().OverrideData(formattedData);
			return row;
		}
	}
}