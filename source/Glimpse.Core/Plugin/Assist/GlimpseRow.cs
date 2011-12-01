using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public class GlimpseRow
	{
		private readonly List<GlimpseColumn> _columns = new List<GlimpseColumn>();

		public IEnumerable<GlimpseColumn> Columns
		{
			get { return _columns; }
		}

		public GlimpseRow Column(object columnData)
		{
			// TODO: Should we throw if column data is null
			var column = new GlimpseColumn(columnData);
			_columns.Add(column);
			return this;
		}

		internal object[] Build()
		{
			return _columns.Select(c => c.Data).ToArray();
		}
	}
}