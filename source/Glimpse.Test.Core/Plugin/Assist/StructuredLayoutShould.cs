using System.Linq;
using Glimpse.Core.Plugin.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class StructuredLayoutShould
	{
		[Fact]
		public void ConstructWithNoRows()
		{
			var layout = StructuredLayout.Create();
			
			var rows = layout.Rows.Count();
			
			Assert.Equal(0, rows);
		}

		[Fact]
		public void ConstructWithSingleRow()
		{
			var layout = StructuredLayout.Create(l => l.Row(r => {}));
			
			var rows = layout.Rows;
			
			Assert.Equal(1, rows.Count());
		}

		[Fact]
		public void ConstructWithTwoRows()
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
		public void AddSingleRow()
		{
			var layout = StructuredLayout.Create().Row(r => {});

			var rows = layout.Rows;

			Assert.Equal(1, rows.Count());
		}

		[Fact]
		public void AddTwoRows()
		{
			var layout = StructuredLayout.Create().Row(r => { }).Row(r => { });

			var rows = layout.Rows;

			Assert.Equal(2, rows.Count());
		}
	}
}
