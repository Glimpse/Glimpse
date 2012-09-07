using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StringFormattingExtensionsFact
	{
		[Fact]
		public void String_Strong_AppliesStrongFormatting()
		{
			var result = String.Strong();

			Assert.Equal(result, @"*Text*");
		}

		[Fact]
		public void String_Emphasis_AppliesEmphasisFormatting()
		{
			var result = String.Emphasis();

			Assert.Equal(result, @"\Text\");
		}

		[Fact]
		public void String_Raw_AppliesRawFormatting()
		{
			var result = String.Raw();

			Assert.Equal(result, @"!Text!");
		}

		[Fact]
		public void String_Sub_AppliesSubFormatting()
		{
			var result = String.Sub();

			Assert.Equal(result, @"|Text|");
		}

		[Fact]
		public void String_Underline_AppliesUnderlineFormatting()
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