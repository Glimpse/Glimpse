using Glimpse.Core.Plugin.Assist;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin.Assist
{
    [TestFixture]
	public class StringFormattingExtensionsTest
    {
        [Test]
		public void String_Bold_AppliesBoldFormatting()
        {
            var result = String.Bold();

			Assert.AreEqual(result, @"*Text*");
        }

		[Test]
		public void String_Italic_AppliesBoldFormatting()
		{
			var result = String.Italic();

			Assert.AreEqual(result, @"\Text\");
		}

    	[Test]
		public void String_Raw_AppliesBoldFormatting()
    	{
			var result = String.Raw();

    		Assert.AreEqual(result, @"!Text!");
    	}

    	[Test]
		public void String_Sub_AppliesBoldFormatting()
		{
			var result = String.Sub();

			Assert.AreEqual(result, @"|Text|");
		}

    	[Test]
		public void String_Underline_AppliesBoldFormatting()
		{
			var result = String.Underline();

			Assert.AreEqual(result, @"_Text_");
		}

		private string String { get; set; }

        [SetUp]
        public void Setup()
        {
			String = "Text";
        }
    }
}
