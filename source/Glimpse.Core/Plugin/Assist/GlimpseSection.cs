using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public class GlimpseSection
	{
		private readonly List<GlimpseRow> _rows = new List<GlimpseRow>();

		public IEnumerable<GlimpseRow> Rows
		{
			get { return _rows; }
		}

		public GlimpseRow AddRow()
		{
			var row = new GlimpseRow();
			_rows.Add(row);
			return row;
		}

		public List<object[]> Build()
		{
			var rootList = new Instance(this);
			rootList.AddRange(_rows.Select(r => r.Build()));
			return rootList;
		}

		internal class Instance : List<object[]>
		{
			public GlimpseSection Data { get; private set; }

			internal Instance(GlimpseSection instance)
			{
				Data = instance;
			}
		}
	}
}