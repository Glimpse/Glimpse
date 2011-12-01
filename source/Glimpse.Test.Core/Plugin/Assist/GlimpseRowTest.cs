using System.Linq;
using Glimpse.Core.Plugin.Assist;
using NUnit.Framework;

namespace Glimpse.Test.Core.Plugin.Assist
{
    [TestFixture]
    public class GlimpseRowTest
    {
        [Test]
		public void GlimpseRow_New_HasNoColumns()
        {
            var columns = Row.Columns;

			Assert.AreEqual(0, columns.Count());
        }

		[Test]
		public void GlimpseRow_Column_AddsColumnAndReturnsSelf()
		{
			var columnObject = new {};
			var row = Row.Column(columnObject);

			Assert.AreEqual(Row, row);
			Assert.AreEqual(1, Row.Columns.Count());
		}

		[Test]
		public void GlimpseRow_Build_ReturnsObjectArrayOfColumnData()
		{
			var columnObject1 = new { Id = "obj1" };
			var columnObject2 = new { Id = "obj2" };
			
			Row.Column(columnObject1).Column(columnObject2);

			var columnData = Row.Build();

			Assert.AreEqual(2, Row.Columns.Count());
			Assert.AreEqual(columnObject1, columnData.ElementAt(0));
			Assert.AreEqual(columnObject2, columnData.ElementAt(1));
		}

		private GlimpseRow Row { get; set; }

        [SetUp]
        public void Setup()
        {
			Row = new GlimpseRow();
        }
    }
}
