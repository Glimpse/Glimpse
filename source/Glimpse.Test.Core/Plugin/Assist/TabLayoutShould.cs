using System.Linq;
using Glimpse.Core.Tab.Assist;
using Xunit;

namespace Glimpse.Test.Core.Plugin.Assist
{
	public class TabLayoutShould
	{
		[Fact]
		public void ConstructWithNoRows()
		{
			var layout = TabLayout.Create();
			
			var rows = layout.Rows.Count();
			
			Assert.Equal(0, rows);
		}

		[Fact]
		public void ConstructWithSingleRow()
		{
			var layout = TabLayout.Create(l => l.Row(r => {}));
			
			var rows = layout.Rows;
			
			Assert.Equal(1, rows.Count());
		}

		[Fact]
		public void ConstructWithTwoRows()
		{
			var layout = TabLayout.Create(l =>
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
			var layout = TabLayout.Create().Row(r => {});

			var rows = layout.Rows;

			Assert.Equal(1, rows.Count());
		}

		[Fact]
		public void AddTwoRows()
		{
			var layout = TabLayout.Create().Row(r => { }).Row(r => { });

			var rows = layout.Rows;

			Assert.Equal(2, rows.Count());
		}
	}
}
