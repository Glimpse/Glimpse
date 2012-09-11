using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StructuredLayoutFact
	{
		[Fact]
		public void StructuredLayout_Create_HasNoRows()
		{
			var layout = StructuredLayout.Create();
			
			var rows = layout.Rows.Count();
			
			Assert.Equal(0, rows);
		}

		[Fact]
		public void StructuredLayout_Create_HasSingleRow()
		{
			var layout = StructuredLayout.Create(l => l.Row(r => {}));
			
			var rows = layout.Rows;
			
			Assert.Equal(1, rows.Count());
		}

		[Fact]
		public void StructuredLayout_Create_HasTwoRows()
		{
			var layout = StructuredLayout.Create(l =>
			{
				l.Row(r => { });
				l.Row(r => { });
			});
			
			var rows = layout.Rows;
			
			Assert.Equal(2, rows.Count());
		}

		[Fact]
		public void StructuredLayout_Row_AddsSingleRow()
		{
			var layout = StructuredLayout.Create().Row(r => {});

			var rows = layout.Rows;

			Assert.Equal(1, rows.Count());
		}

		[Fact]
		public void StructuredLayout_Row_AddsTwoRows()
		{
			var layout = StructuredLayout.Create().Row(r => { }).Row(r => { });

			var rows = layout.Rows;

			Assert.Equal(2, rows.Count());
		}
	}
}
