using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Plugin.Assist;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin.Assist
{
    [TestFixture]
    public class GlimpseSectionTest
    {
        [Test]
		public void GlimpseSection_New_HasNoRows()
        {
            var rows = Section.Rows.Count();

            Assert.AreEqual(0, rows);
        }

		[Test]
		public void GlimpseSection_AddRow_AddsAndReturnsRow()
		{
			var row = Section.AddRow();

			var rows = Section.Rows;

			Assert.AreEqual(1, rows.Count());
			Assert.AreEqual(row, rows.First());
		}

		[Test]
		public void GlimpseSection_Build_ReturnsRowsAsInstance()
		{
			Section.AddRow();

			var section = Section.Build();

			Assert.AreEqual(Section.Rows.Count(), section.Count());
			Assert.AreEqual(typeof(GlimpseSection.Instance), section.GetType());
		}

		[Test]
		public void GlimpseSection_InstanceData_IsSelf()
		{
			var section = Section;
			var sectionInstance = Section.Build() as GlimpseSection.Instance;
			
			Assert.AreEqual(section, sectionInstance.Data);
		}

		[Test]
		public void GlimpseSection_Instance_IsListOfObjectArray()
		{
			Assert.True(typeof(List<object[]>).IsAssignableFrom(typeof(GlimpseSection.Instance)));
		}

		private GlimpseSection Section { get; set; }

        [SetUp]
        public void Setup()
        {
			Section = new GlimpseSection();
        }
    }
}
