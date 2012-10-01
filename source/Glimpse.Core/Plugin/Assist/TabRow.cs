using System;
using System.Collections.Generic;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
    public class TabRow
    {
        private readonly List<TabColumn> _columns = new List<TabColumn>();

        public IEnumerable<TabColumn> Columns
        {
            get { return _columns; }
        }

        public TabRow Column(object columnData)
        {
            if (columnData == null)
                throw new ArgumentNullException("columnData", "Column data must not be null.");
            
            var column = new TabColumn(columnData);
            _columns.Add(column);
            
            return this;
        }

        internal object[] Build()
        {
            return _columns.Select(c => c.Data).ToArray();
        }
    }
}