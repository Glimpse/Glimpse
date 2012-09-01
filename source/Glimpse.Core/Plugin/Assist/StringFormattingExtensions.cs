using System;

namespace Glimpse.Core.Plugin.Assist
{
	public static class StringFormattingExtensions
	{
		public static string Bold(this string value)
		{
			return Formats.Bold.FormatWith(value);
		}

		public static string BoldIf(this string value, bool condition)
		{
			return condition ? value.Bold() : value;
		}

		public static string Underline(this string value)
		{
			return Formats.Underline.FormatWith(value);
		}

		public static string UnderlineIf(this string value, bool condition)
		{
			return condition ? value.Bold() : value;
		}

		public static string FormatWith(this string format, params object[] arguments)
		{
			return String.Format(format, arguments);
		}
	}
}