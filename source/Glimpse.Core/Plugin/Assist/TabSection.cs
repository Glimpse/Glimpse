using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
    public class TabSection
    {
        private readonly List<TabRow> rows = new List<TabRow>();

        public TabSection()
        {
        }

        public TabSection(params string[] headers)
        {
            if (headers.Any())
            {
                var row = AddRow();
                foreach (var header in headers)
                {
                    row.Column(header);
                }
            }
        }

        public IEnumerable<TabRow> Rows
        {
            get { return rows; }
        }

        public TabRow AddRow()
        {
            var row = new TabRow();
            rows.Add(row);
            return row;
        }

        public List<object[]> Build()
        {
            var rootList = new Instance(this);
            rootList.AddRange(rows.Select(r => r.Build()));
            return rootList;
        }

        internal class Instance : List<object[]>
        {
            internal Instance(TabSection instance)
            {
                Data = instance;
            }

            public TabSection Data { get; private set; }
        }
    }
}