using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
	public class TabSection
	{
		private readonly List<GlimpseRow> _rows = new List<GlimpseRow>();

		public TabSection() {}

		public TabSection(params string[] headers)
		{
			if (headers.Any())
			{
				var row = AddRow();
				foreach (var header in headers)
					row.Column(header);
			}
		}

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
			public TabSection Data { get; private set; }

			internal Instance(TabSection instance)
			{
				Data = instance;
			}
		}
	}
}