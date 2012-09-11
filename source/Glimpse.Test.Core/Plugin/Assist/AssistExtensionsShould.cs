using System;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class AssistExtensionsShould
	{
		[Fact]
		public void ThrowWithNull()
		{
			object obj = null;

			Assert.Throws<ArgumentNullException>(() => obj.ToTabSection());
		}

		[Fact]
		public void ThrowWithNonTabSectionObject()
		{
			object obj = new object();

			Assert.Throws<InvalidOperationException>(() => obj.ToTabSection());
		}

		[Fact]
		public void ReturnTabSectionFromGlimpseSection()
		{
			object obj = new TabSection();

			var result = obj.ToTabSection();

			Assert.Equal(obj, result);
		}

		[Fact]
		public void ReturnTabSectionFromTabSectionInstance()
		{
			var section = new TabSection();
			object obj = section.Build();

			var result = obj.ToTabSection();

			Assert.Equal(section, result);
		}
	}
}
