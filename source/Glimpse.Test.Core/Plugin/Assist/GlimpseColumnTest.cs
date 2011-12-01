using Glimpse.Core.Plugin.Assist;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin.Assist
{
    [TestFixture]
    public class GlimpseColumnTest
    {
        [Test]
		public void GlimpseColumn_New_HasData()
        {
            var column = Column;

			Assert.AreEqual(ColumnObject, column.Data);
        }

		[Test]
		public void GlimpseColumn_New_HasGlimpseSectionAsData()
		{
			var section = new GlimpseSection();
			var column = new GlimpseColumn(section);

			Assert.AreEqual(section, column.Data.ToGlimpseSection());
		}

		[Test]
		public void GlimpseColumn_OverrideData_SetsData()
		{
			var columnData = new {};
			var overrideColumnData = new {};
			var column = new GlimpseColumn(columnData);

			column.OverrideData(overrideColumnData);

			Assert.AreEqual(overrideColumnData, column.Data);
		}
		
		private object ColumnObject { get; set; }
		private GlimpseColumn Column { get; set; }

        [SetUp]
        public void Setup()
        {
        	ColumnObject = new { SomeProperty = "SomeValue" };
			Column = new GlimpseColumn(ColumnObject);
        }
    }
}
