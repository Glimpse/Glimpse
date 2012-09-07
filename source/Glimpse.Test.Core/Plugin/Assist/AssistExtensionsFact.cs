using System;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class AssistExtensionsFact
	{
		[Fact]
		public void ToTabSection_null_ShouldThrow()
		{
			object obj = null;

			Assert.Throws<ArgumentNullException>(() => obj.ToTabSection());
		}

		[Fact]
		public void ToTabSection_NonTabSectionObject_ShouldThrow()
		{
			object obj = new object();

			Assert.Throws<InvalidOperationException>(() => obj.ToTabSection());
		}

		[Fact]
		public void ToTabSection_GlimseSection_ReturnsTabSection()
		{
			object obj = new TabSection();

			var result = obj.ToTabSection();

			Assert.Equal(obj, result);
		}

		[Fact]
		public void ToTabSection_TabSectionInstance_ReturnsTabSection()
		{
			var section = new TabSection();
			object obj = section.Build();

			var result = obj.ToTabSection();

			Assert.Equal(section, result);
		}
	}
}
