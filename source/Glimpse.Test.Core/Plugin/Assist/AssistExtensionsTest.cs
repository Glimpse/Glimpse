using System;
using Glimpse.Core.Plugin.Assist;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin.Assist
{
    [TestFixture]
	public class AssistExtensionsTest
    {
		[Test]
		public void ToGlimpseSection_null_ShouldThrow()
		{
			object obj = null;

			Assert.Throws<ArgumentNullException>(() => obj.ToGlimpseSection());
		}

        [Test]
		public void ToGlimpseSection_NonGlimpseSectionObject_ShouldThrow()
        {
            object obj = new object();

			Assert.Throws<InvalidOperationException>(() => obj.ToGlimpseSection());
        }

		[Test]
		public void ToGlimpseSection_GlimseSection_ReturnsGlimpseSection()
		{
			object obj = new GlimpseSection();

			var result = obj.ToGlimpseSection();

			Assert.AreEqual(obj, result);
		}

		[Test]
		public void ToGlimpseSection_GlimseSectionInstance_ReturnsGlimpseSection()
		{
			var section = new GlimpseSection();
			object obj = section.Build();

			var result = obj.ToGlimpseSection();

			Assert.AreEqual(section, result);
		}
    }
}
