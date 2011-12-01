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

		public static string Italic(this string value)
		{
			return Formats.Italic.FormatWith(value);
		}

		public static string ItalicIf(this string value, bool condition)
		{
			return condition ? value.Italic() : value;
		}

		public static string Raw(this string value)
		{
			return Formats.Raw.FormatWith(value);
		}

		public static string RawIf(this string value, bool condition)
		{
			return condition ? value.Raw() : value;
		}

		public static string Sub(this string value)
		{
			return Formats.Sub.FormatWith(value);
		}

		public static string SubIf(this string value, bool condition)
		{
			return condition ? value.Sub() : value;
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