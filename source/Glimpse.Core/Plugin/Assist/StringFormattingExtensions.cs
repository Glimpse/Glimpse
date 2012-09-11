using System;

namespace Glimpse.Core.Plugin.Assist
{
	public static class StringFormattingExtensions
	{
		public static string Strong(this string value)
		{
			return Formats.Strong.FormatWith(value);
		}

		public static string StrongIf(this string value, bool condition)
		{
			return condition ? value.Strong() : value;
		}

		public static string Emphasis(this string value)
		{
			return Formats.Emphasis.FormatWith(value);
		}

		public static string EmphasisIf(this string value, bool condition)
		{
			return condition ? value.Emphasis() : value;
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
			return condition ? value.Underline() : value;
		}

		public static string FormatWith(this string format, params object[] arguments)
		{
			return String.Format(format, arguments);
		}
	}
}