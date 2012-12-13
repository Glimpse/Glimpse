using Glimpse.Core.Tab.Assist;
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
        public void ApplyStrongFormattingIf()
        {
            var result = String.StrongIf(true);

            Assert.Equal(result, @"*Text*");

            result = String.StrongIf(false);

            Assert.Equal(result, @"Text");
        }

		[Fact]
		public void ApplyEmphasisFormatting()
		{
			var result = String.Emphasis();

			Assert.Equal(result, @"\Text\");
		}

        [Fact]
        public void ApplyEmphasisFormattingIf()
        {
            var result = String.EmphasisIf(true);

            Assert.Equal(result, @"\Text\");

            result = String.EmphasisIf(false);

            Assert.Equal(result, @"Text");
        }

		[Fact]
		public void ApplyRawFormatting()
		{
			var result = String.Raw();

			Assert.Equal(result, @"!Text!");
		}

        [Fact]
        public void ApplyRawFormattingIf()
        {
            var result = String.RawIf(true);

            Assert.Equal(result, @"!Text!");

            result = String.RawIf(false);

            Assert.Equal(result, @"Text");
        }

		[Fact]
		public void ApplySubFormatting()
		{
			var result = String.Sub();

			Assert.Equal(result, @"|Text|");
		}

        [Fact]
        public void ApplySubFormattingIf()
        {
            var result = String.SubIf(true);

            Assert.Equal(result, @"|Text|");

            result = String.SubIf(false);

            Assert.Equal(result, @"Text");
        }

		[Fact]
		public void ApplyUnderlineFormatting()
		{
			var result = String.Underline();

			Assert.Equal(result, @"_Text_");
		}

        [Fact]
        public void ApplyUnderlineFormattingIf()
        {
            var result = String.UnderlineIf(true);

            Assert.Equal(result, @"_Text_");

            result = String.UnderlineIf(false);

            Assert.Equal(result, @"Text");
        }

		private string String { get; set; }

		public StringFormattingExtensionsShould()
		{
			String = "Text";
		}
	}
}