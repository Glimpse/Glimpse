using System;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class AssistExtensionsFact
	{
		[Fact]
		public void ToGlimpseSection_null_ShouldThrow()
		{
			object obj = null;

			Assert.Throws<ArgumentNullException>(() => obj.ToGlimpseSection());
		}

		[Fact]
		public void ToGlimpseSection_NonGlimpseSectionObject_ShouldThrow()
		{
			object obj = new object();

			Assert.Throws<InvalidOperationException>(() => obj.ToGlimpseSection());
		}

		[Fact]
		public void ToGlimpseSection_GlimseSection_ReturnsGlimpseSection()
		{
			object obj = new GlimpseSection();

			var result = obj.ToGlimpseSection();

			Assert.Equal(obj, result);
		}

		[Fact]
		public void ToGlimpseSection_GlimseSectionInstance_ReturnsGlimpseSection()
		{
			var section = new GlimpseSection();
			object obj = section.Build();

			var result = obj.ToGlimpseSection();

			Assert.Equal(section, result);
		}
	}
}
