using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StringFormattingExtensionsShould
	{
		[Fact]
		public void ApplyStrongFormatting()
		{
			var result = String.Strong();

			Assert.Equal(result, @"*Text*");
		}

		[Fact]
		public void ApplyEmphasisFormatting()
		{
			var result = String.Emphasis();

			Assert.Equal(result, @"\Text\");
		}

		[Fact]
		public void ApplyRawFormatting()
		{
			var result = String.Raw();

			Assert.Equal(result, @"!Text!");
		}

		[Fact]
		public void ApplySubFormatting()
		{
			var result = String.Sub();

			Assert.Equal(result, @"|Text|");
		}

		[Fact]
		public void ApplyUnderlineFormatting()
		{
			var result = String.Underline();

			Assert.Equal(result, @"_Text_");
		}

		private string String { get; set; }

		public StringFormattingExtensionsShould()
		{
			String = "Text";
		}
	}
}