using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StringFormattingExtensionsFact
	{
		[Fact]
		public void String_Bold_AppliesBoldFormatting()
		{
			var result = String.Bold();

			Assert.Equal(result, @"*Text*");
		}

		[Fact]
		public void String_Italic_AppliesBoldFormatting()
		{
			var result = String.Italic();

			Assert.Equal(result, @"\Text\");
		}

		[Fact]
		public void String_Raw_AppliesBoldFormatting()
		{
			var result = String.Raw();

			Assert.Equal(result, @"!Text!");
		}

		[Fact]
		public void String_Sub_AppliesBoldFormatting()
		{
			var result = String.Sub();

			Assert.Equal(result, @"|Text|");
		}

		[Fact]
		public void String_Underline_AppliesBoldFormatting()
		{
			var result = String.Underline();

			Assert.Equal(result, @"_Text_");
		}

		private string String { get; set; }

		public StringFormattingExtensionsFact()
		{
			String = "Text";
		}
	}
}
