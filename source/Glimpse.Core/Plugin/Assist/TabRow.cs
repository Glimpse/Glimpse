using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public class TabRow
	{
		private readonly List<GlimpseColumn> _columns = new List<GlimpseColumn>();

		public IEnumerable<GlimpseColumn> Columns
		{
			get { return _columns; }
		}

		public TabRow Column(object columnData)
		{
			if (columnData == null)
				throw new ArgumentNullException("columnData", "Column data must not be null.");
			
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