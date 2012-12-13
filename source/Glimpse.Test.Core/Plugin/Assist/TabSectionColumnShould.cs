using Glimpse.Core.Tab.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
    public class TabSectionColumnShould
    {
        [Fact]
        public void ConstructWithData()
        {
            var column = this.SectionColumn;

            Assert.Equal(ColumnObject, column.Data);
        }

        [Fact]
        public void SetData()
        {
            var columnData = new { };
            var overrideColumnData = new { };
            var column = new TabSectionColumn(columnData);

            column.OverrideData(overrideColumnData);

            Assert.Equal(overrideColumnData, column.Data);
        }

        private object ColumnObject { get; set; }
        private TabSectionColumn SectionColumn { get; set; }

        public TabSectionColumnShould()
        {
            ColumnObject = new { SomeProperty = "SomeValue" };
            this.SectionColumn = new TabSectionColumn(ColumnObject);
        }
    }
}
